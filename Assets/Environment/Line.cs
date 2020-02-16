﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Line : MonoBehaviour
{
    [SerializeField]
    int maxLength;
    [SerializeField]
    float lineSpread = 1.2f;
    Vector3[] LineSpots { get; set; }
    protected Queue<ICustomer> line = new Queue<ICustomer>();
    public ICustomer CurrentCustomer
    {
        get
        {
            return line.Count > 0 ? line.Peek() : null;
        }
    }
    public event Action OnCustomerServiced;

    private void Start()
    {
        
    }

    public Vector3 GetProperSpot(int index)
    {
        return LineSpots[index-1];
    }

    public bool isCustomerReady()
    {
        if (CurrentCustomer == null)
        {
            return false;
        }
        float distance = Vector3.Distance(CurrentCustomer.CurrentTransform.position, LineSpots[0]);
        return distance < 3;
    }

    public bool isJoinable(ICustomer customer)
    {
        if (line.Contains(customer))
        {
            return false;
        }
        return !(GetLineLength() == maxLength);
    }

    public int GetLineLength()
    {
        return line.Count;
    }

    protected virtual void GenerateLineSpots()
    {
        LineSpots = new Vector3[maxLength];

        for (int i = 0; i < maxLength; i++)
        {
            LineSpots[i] = new Vector3(transform.position.x, 0, transform.position.z + (i + 1) * lineSpread);
        }
    }

    void OnDrawGizmos()
    {
        if (LineSpots != null)
        {
            foreach (Vector3 item in LineSpots)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(item, 1);
            }
        }
    }

    public int GetFreePosition(ICustomer customer)
    {
        foreach (var item in line)
        {
            if(item == customer)
            {
                return line.ToArray().ToList().IndexOf(item);
            }
        }
        return 1;
    }

    public int JoinLine(ICustomer newCustomer)
    {
        line.Enqueue(newCustomer);
        //Debug.Log($"ENQUED: {newCustomer}");
        int number = GetFreePosition(newCustomer);//GetLineLength();
        return line.Count;
    }

    protected void Dequeue()
    {

        ICustomer customer = line.Dequeue();
        Debug.Log($"Leaved line: {customer}");
        foreach (var item in line)
        {
            Debug.Log($"Still in line: {item.CurrentTransform.name}");
        }
    }

    public void CustomerServicedHandler()
    {
        OnCustomerServiced?.Invoke();
    }
}
