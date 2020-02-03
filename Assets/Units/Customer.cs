using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : Unit, ICustomer
{
    public bool isInLine { get; private set; } = false;
    public CashBox CurrentCashBox { get; private set; }
    public Queue<CashBox> ShoppingRoute { get; private set; }

    public Transform CurrentTransform { get => gameObject.transform; }

    private void Start()
    {
        Initialize();
        stressScript.OnStressOut += StressOutHandler;
        moveScript.OnDestinationReached += ReachedDestinationHandler;
        moveScript.MoveTo(Cafe.Entrance.position);
    }

    void StressOutHandler(object e, EventArgs args)
    {
        Leave(Cafe.Exit.position);
    }

    void ReachedDestinationHandler()
    {
        if (isInLine)
        {
            if(CurrentCashBox.CurrentCustomer.GetHashCode() == this.GetHashCode())
            {
                Debug.Log("HER");
                CurrentCashBox.CustomerReady();
                CurrentCashBox.OnCustomerServiced += GoToNextPoint;
            }
            else
            {
                RotateTowards(CurrentCashBox.worker.transform.position);
            }
        }
        else
        {
            if (ShoppingRoute == null)
            {
                BuildShoppingRoute();
            }
            else
            {
                GoToNextPoint();
            }
        }
    }

    void GoToNextPoint()
    {
        if(ShoppingRoute.Count != 0)
        {
            if(CurrentCashBox != null)
            {
                CurrentCashBox.OnCustomerServiced -= GoToNextPoint;
            }
            CurrentCashBox = ShoppingRoute.Dequeue();
            moveScript.MoveTo(CurrentCashBox.GetPlaceInLine(this));
            isInLine = true;
        }
        else
        {
            isInLine = false;
            Leave(Cafe.Exit.position);
        }
    }

    void BuildShoppingRoute()
    {

        ShoppingRoute = new Queue<CashBox>();
        int departmentsToVisit = UnityEngine.Random.Range(1, Cafe.AllDepartments.Count+1);
        while (departmentsToVisit != 0)
        {
            CashBox new_destination = Cafe.AllDepartments[UnityEngine.Random.Range(0, Cafe.AllDepartments.Count)];
            if (!ShoppingRoute.Contains(new_destination))
            {
                ShoppingRoute.Enqueue(new_destination);
                departmentsToVisit--;
            }
        }
        GoToNextPoint();
    }

    public void Idle()
    {
        throw new System.NotImplementedException();
    }

    public void Leave(Vector3 exitPoint)
    {
        moveScript.MoveTo(exitPoint);
    }

    public void MoveInLine(Vector3 newPosition)
    {
        moveScript.MoveTo(newPosition);
    }

    private void RotateTowards(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));    // flattens the vector3
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 60);
    }

    private void Update()
    {
        //if (isInLine)
        //{
        //    stressScript.IncreaseStress();
        //}
    }
}
