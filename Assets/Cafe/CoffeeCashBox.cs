using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeCashBox : Line
{

    private void Start()
    {
        base.GenerateLineSpots();
        OnCustomerServiced += Dequeue;
    }

}
