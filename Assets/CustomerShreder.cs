using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerShreder : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Customer customer = other.GetComponent<Customer>();
        if(customer != null)
        {
            CustomerPool.PlaceToPool(customer);
        }
    }
}
