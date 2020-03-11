using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OfflineWork : MonoBehaviour
{
    [SerializeField] bool trigger;
    [SerializeField] float value;
    [SerializeField] TextMeshProUGUI textVisual;
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] float reduceMultiplier;
    IdleConfig config;

    // Start is called before the first frame update
    void Start()
    {
        config = FindObjectOfType<IdleConfig>();
        if (PlayerPrefs.GetString(PrefsUtils.onlineDate) != "")
        {
            int cachboxCount = PlayerPrefs.GetInt(PrefsUtils.cashbox) + 1;
            float avSpeed = GetAverageWorkTime(cachboxCount);
            //Debug.Log(avSpeed);
            //Debug.Log(cachboxCount);
            //Debug.Log(config.coinsClickValue);
            CalculateOfflineIncome(avSpeed, cachboxCount, config.coinsClickValue);
        }
        else
        {
            HideVisual();
        }
        StartCoroutine(SaveOnline());
    }


    float GetAverageWorkTime(int cashboxCount)
    {
        CoffeeWorker[] workers = FindObjectsOfType<CoffeeWorker>();
        float totalSpeed = 0;
        foreach (CoffeeWorker worker in workers)
        {
            totalSpeed += worker.GetWorkSpeed();
        }
        return totalSpeed / cashboxCount;
    }

    public void ButtonClicked()
    {
        config.Click(value);
        HideVisual();
    }

    void HideVisual()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }

    void CalculateOfflineIncome(float workSpeed, int cacshBoxes, double cupPrice)
    {
        if (reduceMultiplier == 0)
        {
            reduceMultiplier = 1;
        }
        value = CalculateOfflineTime() * PerSecond(workSpeed, cacshBoxes, cupPrice) / reduceMultiplier;
        value = (float) Math.Round(value, 2);
        textVisual.text = string.Format("Your offline income: {0}", Exponent.SetExponentText(value));
    }

    IEnumerator SaveOnline()
    {
        while (true)
        {
            PlayerPrefs.SetString(PrefsUtils.onlineDate, DateTime.Now.ToString());
            yield return new WaitForSeconds(20);
        }
    }

    float CalculateOfflineTime()
    {
        DateTime last = Convert.ToDateTime(PlayerPrefs.GetString(PrefsUtils.onlineDate));
        return (float)(DateTime.Now - last).TotalSeconds;
    }

    float PerSecond(float workSpeed, int cacshBoxes, double cupPrice)
    {
        return (float)cupPrice/workSpeed * cacshBoxes;
    }
}
