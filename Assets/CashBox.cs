using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CashBox : MonoBehaviour
{
    [SerializeField]
    public CoffeeWorker worker;
    bool proceeding = false;

    Queue<ICustomer> line = new Queue<ICustomer>();
    public ICustomer CurrentCustomer { get; private set; }
    public int LineLength { get => line.Count; }
    float Price { get; }
    public event Action OnCustomerServiced;
    public abstract void StartCustomerService();
    public abstract void MoveLine();

    public Vector3 GetPlaceInLine(ICustomer newCustomer)
    {
        if (line.Count == 0)
        {
            CurrentCustomer = newCustomer;
        }
        line.Enqueue(newCustomer);
        Vector3 newLinePosition = new Vector3(transform.position.x + line.Count * 1.2f, newCustomer.CurrentTransform.position.y, transform.position.z);
        return newLinePosition;
    }

    public void CustomerReady()
    {
        if (!proceeding)
        {
            proceeding = true;
            StartCoroutine(Servicing(worker.WorkSpeed));
        }
    }

    IEnumerator Servicing(float time)
    {
        yield return new WaitForSeconds(time);
        proceeding = false;
        Vector3 prev = CurrentCustomer.CurrentTransform.position;
        OnCustomerServiced?.Invoke();
        if (line.Contains(CurrentCustomer))
        {
            line.Dequeue();
        }
        if (line.Count != 0)
        {
            foreach (ICustomer customer in line)
            {

                customer.MoveInLine(prev);
                prev = customer.CurrentTransform.position;
            }   
            CurrentCustomer = line.Dequeue();
        }
    }
}
