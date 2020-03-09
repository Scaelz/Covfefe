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
        //EnableMenu(false);
    }

    public void EnableMenu(bool state)
    {
        menu.SetActive(state);
    }

}
