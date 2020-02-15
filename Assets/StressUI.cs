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
