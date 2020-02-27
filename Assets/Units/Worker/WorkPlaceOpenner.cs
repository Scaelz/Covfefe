using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WorkPlaceOpenner : MonoBehaviour, IClickable
{
    Material material;
    [SerializeField] int index;
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
        LoadPrefs();
    }

    void LoadPrefs()
    {
        int prefs_index = PlayerPrefs.GetInt(PrefsUtils.cashbox);
        if(prefs_index >= index)
        {
            Debug.Log("oppening");
            OpenNewWorkPlace(silent: true);
        }
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
            OpenNewWorkPlace(silent: false);
        }
    }

    public void OpenNewWorkPlace(bool silent = false)
    {
        float activationTime = silent ? deactivationDelay : 0;

        priceText.enabled = false;
        renderer.enabled = false;
        StartCoroutine(DelayedDeactivate(deactivationDelay));
        ActivateWorkPlace();
        if (!silent)
        {
            PlayEffect();
            config.SpentCoins(workPlace.price);
            SaveProgress();
        }
    }

    void SaveProgress()
    {
        PlayerPrefs.SetInt(PrefsUtils.cashbox, index);
        PlayerPrefs.Save();
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
