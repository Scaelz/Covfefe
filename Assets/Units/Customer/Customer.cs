using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Customer : Unit, ICustomer, ILineable
{
    bool withCoffe = false;
    bool routeBuilt = false;
    public Queue<Line> ShoppingRoute { get; private set; }
    public Transform CurrentTransform { get => gameObject.transform; }
    public bool isInLine { get; private set; } = false;
    public int PositionInLine { get; private set; }
    public Line CurrentLine { get; private set; }
    
    [SerializeField]
    CustomerAnimator animation;

    private void Start()
    {
        animation = GetComponent<CustomerAnimator>();
        moveScript.OnDestinationReached += LookInLine;
        moveScript.OnDestinationReached += SetIdleAnimation;
        moveScript.OnStartMoving += SetWalkingAnimation;
    }

    void SetWalkingAnimation()
    {
        if (withCoffe)
        {
            animation.Play(ClientAnims.take);
        }
        else
        {
            animation.Play(ClientAnims.walk);
        }
    }

    void SetIdleAnimation()
    {
        animation.Play(ClientAnims.idle);
    }

    void LookInLine()
    {
        if (CurrentLine != null)
        {
            moveScript.RotateTowards(CurrentLine.transform.position);
        }
    }

    private void OnEnable()
    {
        Initialize();
        LeaveLine();
        withCoffe = false;
        //stressScript.OnStressOut += StressOutHandler;
        BuildShoppingRoute();
        moveScript.SetPriority(50);
        moveScript.MoveTo(Cafe.Entrance.position);
    }

    void Shopping()
    {
        if(ShoppingRoute.Count == 0)
        {
            Leave();
        }
        else
        {
            Line line = ShoppingRoute.Dequeue();
            JoinLine(line);
        }
    }

    public void Leave()
    {
        moveScript.MoveTo(Cafe.GetExitPoint());
    }

    void StressOutHandler(object e, EventArgs args)
    {
        Leave();
    }

    public void JoinLine(Line line)
    {
        PositionInLine = line.JoinLine(this);
        if (PositionInLine != -1)
        {
            CurrentLine = line;
            CurrentLine.OnCustomerServiced += MoveInLine;
            isInLine = true;
            Vector3 lineSpot = line.GetProperSpot(PositionInLine);
            moveScript.MoveTo(lineSpot);
        }
    }

    public void LeaveLine()
    {
        if (CurrentLine != null)
        {
            CurrentLine.OnCustomerServiced -= MoveInLine;
        }
        CurrentLine = null;
        isInLine = false;
        PositionInLine = -1;
    }

    public void MoveInLine()
    {
        if(CurrentLine != null)
        {
            if (PositionInLine == 0)
            {
                LeaveLine();
                withCoffe = true;
                moveScript.SetPriority(1);
                Shopping();
            }
            else
            {
                PositionInLine--;// = CurrentLine.GetFreePosition(this);
                Vector3 newPosition = CurrentLine.GetProperSpot(PositionInLine);
                moveScript.MoveTo(newPosition);
            }
        }
    }

    void BuildShoppingRoute()
    {
        ShoppingRoute = new Queue<Line>();
        
        Line new_destination = Cafe.AllDepartments.OrderBy(x => x.GetLineLength()).FirstOrDefault();
        if (!ShoppingRoute.Contains(new_destination))
        {
            ShoppingRoute.Enqueue(new_destination);
        }
        //int departmentstovisit = 1;// UnityEngine.Random.Range(1, Cafe.AllDepartments.Count + 1);
        //while (departmentstovisit != 0)
        //{
        //    Line new_destination = Cafe.AllDepartments.OrderBy(x => x.GetLineLength()).FirstOrDefault();
        //    //Line new_destination = Cafe.AllDepartments[UnityEngine.Random.Range(0, Cafe.AllDepartments.Count)];
        //    if (!ShoppingRoute.Contains(new_destination))
        //    {
        //        ShoppingRoute.Enqueue(new_destination);
        //        departmentstovisit--;
        //    }
        //}
        routeBuilt = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Finish")
        {
            Shopping();
            if (!isInLine)
            {
                BuildShoppingRoute();
                Shopping();
            }
            if (!isInLine)
            {
                Leave();
            }
        }
    }
}
