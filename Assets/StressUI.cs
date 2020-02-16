using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StressUI : MonoBehaviour
{
    [SerializeField]
    Slider rageSlider;
    [SerializeField]
    GameObject stressAbleObject;
    IStressable stress;
    [SerializeField]
    Camera cam;

    private void LateUpdate()
    {
        transform.LookAt(transform.position + cam.transform.forward) ;
    }

    // Start is called before the first frame update
    void Start()
    {
        stress = stressAbleObject.GetComponent<IStressable>();
        rageSlider.maxValue = stress.MaxStress;

        stress.OnStressChanged += SetStressSliderValue;
    }
   
    void SetStressSliderValue(float value)
    {
        rageSlider.value = value;
    }
}
