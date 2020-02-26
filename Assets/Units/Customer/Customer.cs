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
    bool happyTrigger = false;
    public Queue<Line> ShoppingRoute { get; private set; }
    public Transform CurrentTransform { get => gameObject.transform; }
    public bool isInLine { get; private set; } = false;
    public int PositionInLine { get; private set; }
    public Line CurrentLine { get; private set; }
    public event Action OnHappy;
    public event Action OnRage;
    public event Action OnEnabling;
    [SerializeField]
    CustomerAnimator anim;
    
    private void Start()
    {
        //anim = GetComponent<CustomerAnimator>();
        moveScript.OnDestinationReached += LookInLine;
        moveScript.OnDestinationReached += SetIdleAnimation;
        moveScript.OnStartMoving += SetWalkingAnimation;
        OnHappy += RecommendPlace;
        OnRage += UnRecommendPlace;
    }
    
    void RecommendPlace()
    {
        PopularitySystem.Instance.AddPopularity();
    }

    void UnRecommendPlace()
    {
        PopularitySystem.Instance.DecreasePopularity();
    }

    void SetWalkingAnimation()
    {
        if (withCoffe)
        {
            anim.Play(ClientAnims.take);
        }
        else
        {
            anim.Play(ClientAnims.walk);
        }
    }

    void SetIdleAnimation()
    {
        anim.Play(ClientAnims.idle);
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
        routeBuilt = false;
        Initialize();
        LeaveLine();
        withCoffe = false;
        happyTrigger = false;
        SetWalkingAnimation();
        //stressScript.OnStressOut += StressOutHandler;
        //BuildShoppingRoute();
        moveScript.SetPriority(50);
        OnEnabling?.Invoke();
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
            happyTrigger = UnityEngine.Random.value < 0.35f ? true : false;
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
        if (happyTrigger)
        {
            OnHappy?.Invoke();
        }
    }

    public void MoveInLine()
    {
        if(CurrentLine != null)
        {
            if (PositionInLine == 0)
            {
                withCoffe = true;
                moveScript.MoveTo(CurrentLine.GetLeaveSpot());
                LeaveLine();
                //moveScript.SetPriority(1);
                //Shopping();
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

        //Line new_destination = Cafe.AllDepartments.OrderBy(x => x.GetLineLength()).FirstOrDefault();
        var new_destination = Cafe.AllDepartments.OrderBy(x => x.GetLineLength());
        if (!ShoppingRoute.Contains(new_destination.FirstOrDefault()))
        {
            ShoppingRoute.Enqueue(new_destination.FirstOrDefault());
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
            if (!routeBuilt)
            {
                BuildShoppingRoute();
            }
            Shopping();
            //if (!isInLine)
            //{
            //    BuildShoppingRoute();
            //    Shopping();
            //}
            if (!isInLine)
            {
                OnRage?.Invoke();
                Leave();
            }
        }
        else if (other.tag == "Leave")
        {
            Leave();
        }
    }
}
