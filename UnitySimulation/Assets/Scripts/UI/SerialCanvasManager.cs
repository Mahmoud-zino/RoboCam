using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SerialCanvasManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown serialPortsDropDown;
    [SerializeField]
    private TMP_Text connectionButtonText;
    [SerializeField]
    private TMP_Text disconnectionButtonText;
    [SerializeField]
    private Button sendButton;
    [Header("Serial Settings")]
    [SerializeField]
    private TMP_Dropdown baudRateDropDown;
    [SerializeField]
    private TMP_Dropdown parityDropDown;
    [SerializeField]
    private TMP_Dropdown dataBitsDropDown;
    [SerializeField]
    private TMP_Dropdown StopBitsDropDown;

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

    private void ConnectSerially()
    {
        string portName = serialPortsDropDown.options[serialPortsDropDown.value].text;
        int baudRate = int.Parse(baudRateDropDown.options[baudRateDropDown.value].text);
        Parity parity = (Parity)parityDropDown.value;
        int dataBits = dataBitsDropDown.value == 0 ? 7 : 8;
        StopBits stopBits = StopBitsDropDown.value == 0 ? StopBits.One : StopBits.Two;

        SerialConnectionManager.Instance.Connect(portName, baudRate, parity, dataBits, stopBits);
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

            ConnectSerially();

            TriggerConnectionTextVisibility(false);

            StartCoroutine(CheckConnectionRoutine());

            //Send movement command
            //GameObject.Find("Robot").GetComponent<ManualMovementController>().OnSendPositionClick(sendButton);
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
