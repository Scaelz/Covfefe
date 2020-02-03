using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cafe : MonoBehaviour
{
    public Transform exit;
    public static Transform Exit { get; private set; }
    public Transform entrance;
    public static Transform Entrance { get; private set; }

    public static List<CashBox> AllDepartments { get; private set; } = new List<CashBox>();

    private void Awake()
    {
        Exit = exit;
        Entrance = entrance;
        foreach (CashBox cashbox in FindObjectsOfType<CashBox>())
        {
            AllDepartments.Add(cashbox);
        }
    }

    private void Update()
    {
    }
}
