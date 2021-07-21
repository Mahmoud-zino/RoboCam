using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderValueDisplay : MonoBehaviour
{
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private TMP_Text valueField;
    [SerializeField]
    private float resetPosition;

    private float nullPosition;

    private void Start()
    {
        nullPosition = slider.value;
    }

    private void Update()
    {
        valueField.text = $"{slider.value}°";
    }

    public void ResetPosition()
    {
        slider.value = resetPosition;
    }

    public void NullPosition()
    {
        slider.value = nullPosition;
    }
}
