using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeEnableUI : MonoBehaviour
{
    [SerializeField] GameObject menu;
    [SerializeField] bool menuEnabled = false;

    // Start is called before the first frame update
    void Start()
    {
        //menu.SetActive(menuEnabled);
    }
    public void OnEnabled()
    {
        if (menuEnabled) menuEnabled = false; else menuEnabled = true;

        menu.SetActive(menuEnabled);
    }
}
