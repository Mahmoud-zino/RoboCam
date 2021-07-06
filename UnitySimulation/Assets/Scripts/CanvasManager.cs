using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(1000)]
//if this script loads early it will cause an error
//TODO: a solution could be a min 2 seconds splash screen // Google it 
public class CanvasManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown serialPortsDropDown;
    [SerializeField]
    private TMP_Text connectionButtonText;
    [SerializeField]
    private TMP_Text disconnectionButtonText;

    private void Start()
    {
        UpdateComList();
    }

    //Refreshes the list of available serial Ports
    public void UpdateComList()
    {
        serialPortsDropDown.options.Clear();
        foreach (string port in SerialConnectionManager.Instance.GetAvailablePorts())
        {
            serialPortsDropDown.options.Add(new TMP_Dropdown.OptionData(port));
        }
    }

    public void TriggerSerialConnection()
    {
        //Connecting
        if(connectionButtonText.gameObject.activeSelf)
        {
            if (serialPortsDropDown.value < 0)
            {
                Debug.LogError("Choose an Port Before connecting!");
                return;
            }
            SerialConnectionManager.Instance.Connect(serialPortsDropDown.options[serialPortsDropDown.value].text);

            connectionButtonText.gameObject.SetActive(false);
            disconnectionButtonText.gameObject.SetActive(true);
            StartCoroutine(CheckConnectionRoutine());
        }
        //Disconnecting
        else
        {
            SerialConnectionManager.Instance.CloseConnection();

            connectionButtonText.gameObject.SetActive(true);
            disconnectionButtonText.gameObject.SetActive(false);
        }
    }

    private IEnumerator CheckConnectionRoutine()
    {
        yield return new WaitForSeconds(2);

        if (SerialConnectionManager.Instance.IsConnected())
        {
            StartCoroutine(CheckConnectionRoutine());
        }
        else
        {
            Debug.Log("Connection Lost!");
            TriggerSerialConnection();
        }
    }
}
