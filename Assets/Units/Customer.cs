using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : Unit, ICustomer
{
    public bool isInLine { get; private set; } = false;

    public Queue<Vector3> ShoppingRoute { get; private set; }

    private void Start()
    {
        Initialize();
        stressScript.OnStressOut += StressOutHandler;
        GoShoping();
    }

    void StressOutHandler(object e, EventArgs args)
    {
        Leave();
    }

    public void GetInLine()
    {
        throw new System.NotImplementedException();
    }

    void BuildShoppingRoute()
    {
        ShoppingRoute = new Queue<Vector3>();
        int departmentsToVisit = UnityEngine.Random.Range(1, Cafe.AllDepartments.Count+1);
        while (departmentsToVisit != 0)
        {
            Vector3 new_destination = Cafe.AllDepartments[UnityEngine.Random.Range(0, Cafe.AllDepartments.Count)].position;
            if (!ShoppingRoute.Contains(new_destination))
            {
                ShoppingRoute.Enqueue(new_destination);
                departmentsToVisit--;
            }
        }
        ShoppingRoute.Enqueue(Cafe.Exit.position);
    }

    public void GoShoping()
    {
        if(ShoppingRoute == null)
        {
            BuildShoppingRoute();
        }
        foreach (Vector3 point in ShoppingRoute)
        {
            Debug.Log(point);
        }
    }

    public void Idle()
    {
        throw new System.NotImplementedException();
    }

    public void Leave()
    {
        moveScript.MoveTo(Cafe.Exit.position);
    }

    public void MoveInLine()
    {
        Debug.Log("Step forward in line");
    }

    private void RotateTowards(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));    // flattens the vector3
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 60);
    }

    private void Update()
    {
        if (isInLine)
        {
            stressScript.IncreaseStress();
        }
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 point = ShoppingRoute.Dequeue();
            moveScript.MoveTo(point);
            RotateTowards(Cafe.Exit.position);
        }
    }
}
