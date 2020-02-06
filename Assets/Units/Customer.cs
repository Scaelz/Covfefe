using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : Unit, ICustomer, ILineable
{
    bool routeBuilt = false;
    public Queue<Line> ShoppingRoute { get; private set; }

    public Transform CurrentTransform { get => gameObject.transform; }

    public bool isInLine { get; private set; } = false;
    public int PositionInLine { get; private set; }
    public Line CurrentLine { get; private set; }

    private void Start()
    {
        

    }

    private void OnEnable()
    {
        Initialize();
        stressScript.OnStressOut += StressOutHandler;
        BuildShoppingRoute();
        moveScript.SetPriority(50);
        moveScript.MoveTo(Cafe.Entrance.position);
    }

    void Shopping()
    {
        if(ShoppingRoute.Count == 0)
        {
            moveScript.MoveTo(Cafe.Exit.position);
        }
        else
        {
            Line line = ShoppingRoute.Dequeue();
            JoinLine(line);
        }
    }

    public void Leave(Vector3 exitPoint)
    {
        moveScript.MoveTo(exitPoint);
    }

    void StressOutHandler(object e, EventArgs args)
    {
        Leave(Cafe.Exit.position);
    }

    public void JoinLine(Line line)
    {
        if (line.isJoinable())
        {
            CurrentLine = line;
            PositionInLine = line.JoinLine(this);
            Vector3 lineSpot = line.GetProperSpot(PositionInLine);
            CurrentLine.OnCustomerServiced += MoveInLine;
            moveScript.MoveTo(lineSpot);
            isInLine = true;
        }
    }

    public void LeaveLine()
    {
        CurrentLine.OnCustomerServiced -= MoveInLine;
        CurrentLine = null;
        isInLine = false;
        PositionInLine = -1;
    }

    public void MoveInLine()
    {
        if (PositionInLine == 1)
        {
            LeaveLine();
            Shopping();
        }
        else
        {
            PositionInLine--;
            Vector3 newPosition = CurrentLine.GetProperSpot(PositionInLine);
            moveScript.MoveTo(newPosition);
        }
    }

    void BuildShoppingRoute()
    {
        ShoppingRoute = new Queue<Line>();
        int departmentstovisit = UnityEngine.Random.Range(1, Cafe.AllDepartments.Count + 1);
        while (departmentstovisit != 0)
        {
            Line new_destination = Cafe.AllDepartments[UnityEngine.Random.Range(0, Cafe.AllDepartments.Count)];
            if (!ShoppingRoute.Contains(new_destination))
            {
                ShoppingRoute.Enqueue(new_destination);
                departmentstovisit--;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Shopping();
        if (!isInLine)
        {
            moveScript.SetPriority(1);
            moveScript.MoveTo(Cafe.Exit.position);
        }
    }

    //void ReachedDestinationHandler()
    //{
    //    if (isInLine)
    //    {
    //        if(CurrentCashBox.CurrentCustomer.GetHashCode() == this.GetHashCode())
    //        {
    //            CurrentCashBox.CustomerReady();
    //            CurrentCashBox.OnCustomerServiced += GoToNextPoint;
    //        }
    //        else
    //        {
    //            moveScript.RotateTowards(CurrentCashBox.worker.transform.position);
    //        }
    //    }
    //    else
    //    {
    //        if (ShoppingRoute == null)
    //        {
    //            BuildShoppingRoute();
    //        }
    //        else
    //        {
    //            GoToNextPoint();
    //        }
    //    }
    //}

    //void GoToNextPoint()
    //{
    //    if(ShoppingRoute.Count != 0)
    //    {
    //        if(CurrentCashBox != null)
    //        {
    //            CurrentCashBox.OnCustomerServiced -= GoToNextPoint;
    //        }
    //        CurrentCashBox = ShoppingRoute.Dequeue();
    //        moveScript.MoveTo(CurrentCashBox.GetPlaceInLine(this));
    //        isInLine = true;
    //    }
    //    else
    //    {
    //        isInLine = false;
    //        Leave(Cafe.Exit.position);
    //    }
    //}



    //public void Idle()
    //{
    //    throw new System.NotImplementedException();
    //}


    //public void MoveInLine(Vector3 newPosition)
    //{
    //    moveScript.MoveTo(newPosition);
    //}

    //private void Update()
    //{
    //    //if (isInLine)
    //    //{
    //    //    stressScript.IncreaseStress();
    //    //}
    //}
}
