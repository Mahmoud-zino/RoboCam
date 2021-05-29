using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderValueDisplay : MonoBehaviour
{
    public Slider slider;
    public Text valueField;

    private float startValue;

    private void Start()
    {
        startValue = slider.value;
    }

    private void Update()
    {
        valueField.text = $"{slider.value}°";
    }

    public void ResetPosition()
    {
        slider.value = startValue;
    }
}
