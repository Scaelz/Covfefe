using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Customer : Unit, ICustomer, ILineable, IUpgradeable
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
        TestUpgradeSystem.UpgradeRequest(GetType(), this);
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
        //LeaveLine();
        withCoffe = false;
        happyTrigger = false;
        SetWalkingAnimation();
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
            }
            else
            {
                PositionInLine--;
                Vector3 newPosition = CurrentLine.GetProperSpot(PositionInLine);
                moveScript.MoveTo(newPosition);
            }
        }
    }

    void BuildShoppingRoute()
    {
        ShoppingRoute = new Queue<Line>();
        var new_destination = Cafe.AllDepartments.OrderBy(x => x.GetLineLength());
        if (!ShoppingRoute.Contains(new_destination.FirstOrDefault()))
        {
            ShoppingRoute.Enqueue(new_destination.FirstOrDefault());
        }
        routeBuilt = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "CollisionTag")
        {
            moveScript.SetQuality(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "CollisionTag")
        {
            moveScript.SetQuality(true);
        }
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

    public void Upgrade(CustomUpgrade upgrade, int lvl, int maxLvl)
    {
        switch (upgrade)
        {
            case CustomUpgrade.CustomerSpeed:
                ApplySpeedUpgrade(lvl, maxLvl);
                break;
            case CustomUpgrade.CustomerHappyRate:
                break;
            case CustomUpgrade.CustomerHappyPower:
                break;
            default:
                break;
        }
    }

    void ApplySpeedUpgrade(int lvl, int maxLvl)
    {
        float upgradeTick = (moveScript.GetMaxSpeed() - moveScript.GetDefaultSpeed())/maxLvl;
        float newSpeed = moveScript.GetDefaultSpeed(); 
        for (int i = 0; i < lvl; i++)
        {
            newSpeed += upgradeTick;
        }
        moveScript.ChangeSpeed(newSpeed);
    }
}
