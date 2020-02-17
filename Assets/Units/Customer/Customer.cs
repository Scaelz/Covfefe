using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Customer : Unit, ICustomer, ILineable
{
    [SerializeField]
    TextMeshProUGUI tmp;
    [SerializeField]
    Camera cam;


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
        cam = Camera.main;
        animation = GetComponent<CustomerAnimator>();
        moveScript.OnDestinationReached += LookInLine;
        moveScript.OnDestinationReached += SetIdleAnimation;
        moveScript.OnStartMoving += SetWalkingAnimation;
    }

    void SetWalkingAnimation()
    {
        animation.Play(ClientAnims.walk);
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
        //stressScript.OnStressOut += StressOutHandler;
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
        if (other.tag == "Finish")
        {
            Shopping(); 
            if (!isInLine)
            {
                Debug.Log($"Leaving: {transform.name}");
                
                moveScript.SetPriority(1);

                moveScript.MoveTo(Cafe.Exit.position);
            }
        }
    }

    private void LateUpdate()
    {
        tmp.text = transform.name;
        tmp.transform.parent.LookAt(tmp.transform.parent.position + cam.transform.forward);
    }
}
