using System.Collections.Generic;
using UnityEngine;

public class CustomerPool : MonoBehaviour
{
    public static CustomerPool Instance;
    static Queue<Customer> poolQueue = new Queue<Customer>();
    [SerializeField]
    Customer prefab;

    private void Awake()
    {
        Instance = this;
    }

    public Customer Get()
    {
        Customer customer;
        if (poolQueue.Count == 0)
        {
            customer = CreateInstance();
        }
        else
        {
            customer = poolQueue.Dequeue();
        }
        //customer.gameObject.SetActive(true);
        return customer;
    }

    Customer CreateInstance()
    {
        return Instantiate(Instance.prefab);
    }

    public void AddToPool(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Customer customer = CreateInstance();
            PlaceToPool(customer);
        }
    }

    public static void PlaceToPool(Customer customer)
    {
        customer.gameObject.SetActive(false);
        poolQueue.Enqueue(customer);
    }
}
