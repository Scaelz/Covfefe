using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomerPool : MonoBehaviour
{
    public static CustomerPool Instance;
    static Queue<Customer> poolQueue = new Queue<Customer>();
    [SerializeField]
    Customer[] prefab;
    private static System.Random random = new System.Random();
    public static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
          .Select(s => s[random.Next(s.Length)]).ToArray());
    }
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
            customer.CurrentTransform.name = RandomString(5);
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
        Customer new_prefab = prefab[Random.Range(0, prefab.Length)];
        return Instantiate(new_prefab);
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
