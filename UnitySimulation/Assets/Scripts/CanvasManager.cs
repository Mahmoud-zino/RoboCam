using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown serialPortsDropDown;

    private void Start()
    {
        UpdateComList(serialPortsDropDown);
    }

    //Refreshes the list of available serial Ports
    public void UpdateComList(TMP_Dropdown dropdown)
    {
        dropdown.options.Clear();
        foreach (string port in SerialConnectionController.Instance.GetAvailablePorts())
        {
            dropdown.options.Add(new TMP_Dropdown.OptionData(port));
        }
    }
}
