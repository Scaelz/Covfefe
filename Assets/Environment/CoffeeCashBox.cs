using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeCashBox : Line
{

    private void Update()
    {
        foreach (var item in line)
        {
           //Debug.Log($"{item.CurrentTransform.name}");
        }
    }

    private void Start()
    {
        base.GenerateLineSpots();
        OnCustomerServiced += Dequeue;
    }

}
