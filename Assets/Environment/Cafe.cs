﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cafe : MonoBehaviour
{
    public Transform exit;
    public static Transform Exit { get; private set; }
    public Transform entrance;
    public static Transform Entrance { get; private set; }

    public static List<Line> AllDepartments { get; private set; } = new List<Line>();

    private void Awake()
    {
        Exit = exit;
        Entrance = entrance;
        foreach (Line cashbox in FindObjectsOfType<Line>())
        {
            AllDepartments.Add(cashbox);
        }
    }

    private void Update()
    {
    }
}