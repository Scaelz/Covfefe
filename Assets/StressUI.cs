using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StressUI : MonoBehaviour
{
    [SerializeField]
    Slider rageSlider;
    [SerializeField]
    RawImage rageImageBar;
    [SerializeField]
    Gradient gradient;
    [SerializeField]
    GameObject stressAbleObject;
    IStressable stress;
    [SerializeField]
    Camera cam;
    Canvas canvas;
    CanvasGroup canvasGroup;
    [SerializeField]
    float hideStressBarSpeed = 0.07f;


    private void LateUpdate()
    {
        transform.LookAt(transform.position + cam.transform.forward) ;
    }

    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponent<Canvas>();
        cam = Camera.main;
        canvas.worldCamera = cam;
        stress = stressAbleObject.GetComponent<IStressable>();
        rageSlider.maxValue = stress.MaxStress;

        stress.OnStressChanged += SetStressSliderValue;
        stress.OnStressChanged += StressValueChangedHandler;
        stress.OnStressChanged += SetStressBarColor;
    }
   
    void SetStressSliderValue(float value)
    {
        rageSlider.value = value;
    }

    void SetStressBarColor(float value)
    {
        rageImageBar.color = gradient.Evaluate(rageSlider.normalizedValue);
    }

    void StressValueChangedHandler(float value)
    {
        if (value < 0.03f)
        {
            HideStressBar();
        }
        else
        {
            ShowStressBar();
        }
    }

    void HideStressBar()
    {
        if (canvasGroup.alpha != 0)
        {
            StopAllCoroutines();
            StartCoroutine(LerpAlpha(0, hideStressBarSpeed));
        }
    }

    void ShowStressBar()
    {
        if (canvasGroup.alpha < 0.07f)
        {
            StopAllCoroutines();
            StartCoroutine(LerpAlpha(1, hideStressBarSpeed));
        }
    }

    IEnumerator LerpAlpha(float lerpTo, float lerpSpeed)
    {
        float canvasAlpha = canvasGroup.alpha;
        while (Mathf.Abs(canvasAlpha - lerpTo) > 0.05f)
        {
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, lerpTo, lerpSpeed);
            yield return null;
        }
    }
}
