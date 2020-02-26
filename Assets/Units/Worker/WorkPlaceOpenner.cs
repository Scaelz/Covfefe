﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WorkPlaceOpenner : MonoBehaviour, IClickable
{
    Material material;
    [SerializeField] WorkPlace workPlace;
    [SerializeField] GameObject workPlaceObject;
    [SerializeField] Color[] colors;
    [SerializeField] TextMeshPro priceText;
    [SerializeField] ParticleSystem pSystem;
    [SerializeField] float deactivationDelay;
    MeshRenderer renderer;
    IdleConfig config;
    Coins coins;
    bool state = false;

    private void Start()
    {
        config = FindObjectOfType<IdleConfig>();
        coins = FindObjectOfType<Coins>();
        renderer = GetComponent<MeshRenderer>();
        material = renderer.material;
        config.OnAddCoins += CoinsAddedHandler;
        config.OnMinusCoins += CoinsAddedHandler;
        SetConvertedPrice(workPlace.price);
        CoinsAddedHandler(0);
    }

    void SetConvertedPrice(float price)
    {
        priceText.text = price.ToString();
    }

    void CoinsAddedHandler(double value)
    {
        double coins_value = coins.GetCoins();
        bool state = false;
        if (coins_value > workPlace.price)
        {
            state = true;
        }
        ChangeOpenButtonState(state);
    }

    void ChangeOpenButtonState(bool state)
    {
        Color c = state ? colors[1] : colors[0];
        this.state = state;
        ChangeButtonColor(c);
    }

    void ChangeButtonColor(Color color)
    {
        material.color = color;
    }

    public void OnDown()
    {
        Debug.Log("Down");
    }

    public void OnHold()
    {
        //Debug.Log("Oppener Hold");
    }

    public void OnUp()
    {
        if (state)
        {
            priceText.enabled = false;
            renderer.enabled = false;
            StartCoroutine(DelayedDeactivate(deactivationDelay));
            ActivateWorkPlace();
            PlayEffect();
        }
    }

    IEnumerator DelayedDeactivate(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }

    void ActivateWorkPlace()
    {
        workPlaceObject.SetActive(true);
        Cafe.RefreshDepartments();
    }

    void PlayEffect()
    {
        pSystem.Play();
    }
}
