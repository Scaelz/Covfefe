using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeCashBox : Line
{

    private void Update()
    {
    }

    private void Start()
    {
        base.GenerateLineSpots();
        OnCustomerServiced += Dequeue;
    }

}
