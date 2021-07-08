using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown serialPortsDropDown;
    [SerializeField]
    private TMP_Text connectionButtonText;
    [SerializeField]
    private TMP_Text disconnectionButtonText;
    [SerializeField]
    private Button sendButton;

    private void Start()
    {
        UpdateComList();
    }

    private void Update()
    {
        sendButton.gameObject.SetActive(SerialConnectionManager.Instance.IsConnected());
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
            TriggerConnectionTextVisibility(false);

            StartCoroutine(CheckConnectionRoutine());

            //Send movement command
            GameObject.Find("Robot").GetComponent<RobotManualMovementController>().OnSendPositionClick(sendButton);
        }
        //Disconnecting
        else
        {
            SerialConnectionManager.Instance.CloseConnection();
            TriggerConnectionTextVisibility(true);
        }
    }

    private void TriggerConnectionTextVisibility(bool connected)
    {
        connectionButtonText.gameObject.SetActive(connected);
        disconnectionButtonText.gameObject.SetActive(!connected);
    }


    private IEnumerator CheckConnectionRoutine()
    {
        yield return new WaitForSecondsRealtime(2);

        if (SerialConnectionManager.Instance.IsConnected())
        {
            StartCoroutine(CheckConnectionRoutine());
        }
        else
        {
            Debug.Log("Connection Lost!");
            SerialConnectionManager.Instance.CloseConnection();
            TriggerConnectionTextVisibility(true);
        }
    }
}
