using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cafe : MonoBehaviour
{
    public Transform exit;
    public static Transform Exit { get; private set; }

    public List<Transform> departments;
    public static List<Transform> AllDepartments { get; private set; }

    private void Start()
    {
        Exit = exit;
        AllDepartments = departments;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

        }
    }
}
