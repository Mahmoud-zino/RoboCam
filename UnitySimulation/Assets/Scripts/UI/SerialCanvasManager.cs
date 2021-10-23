using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
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
        StartCoroutine(UpdateComList());
    }

    private void Update()
    {
        sendButton.gameObject.SetActive(SerialConnectionManager.Instance.IsConnected());
    }

    //Refreshes the list of available serial Ports
    private IEnumerator UpdateComList()
    {
        yield return new WaitForSecondsRealtime(1);

        List<string> comPorts = SerialConnectionManager.Instance.GetAvailablePorts();

        //Remove Unlisted Ports
        foreach (TMP_Dropdown.OptionData option in serialPortsDropDown.options.ToList())
        {
            if (!comPorts.Contains(option.text))
                serialPortsDropDown.options.Remove(option);
        }


        List<string> options = serialPortsDropDown.options.Select(x => x.text).ToList();

        //Add new Listed Porst
        foreach (string port in comPorts)
        {
            if(!options.Contains(port))
                serialPortsDropDown.options.Add(new TMP_Dropdown.OptionData(port));
        }

        StartCoroutine(UpdateComList());
    }

    private void ConnectSerially()
    {
        if (serialPortsDropDown.options.Count <= serialPortsDropDown.value)
        {
            Logger.Log.Error("Selected Port is not active any more!");
            return;
        }

        string portName = serialPortsDropDown.options[serialPortsDropDown.value].text;
        int baudRate = int.Parse(baudRateDropDown.options[baudRateDropDown.value].text);
        Parity parity = (Parity)parityDropDown.value;
        int dataBits = dataBitsDropDown.value == 0 ? 7 : 8;
        StopBits stopBits = StopBitsDropDown.value == 0 ? StopBits.One : StopBits.Two;

        try
        {
            SerialConnectionManager.Instance.Connect(portName, baudRate, parity, dataBits, stopBits);
        }
        catch
        {
            Logger.Log.Warning($"Failed to connect to port: {portName}!");
        }
    }

    public void TriggerSerialConnection()
    {
        //Connecting
        if(connectionButtonText.gameObject.activeSelf)
        {
            if (serialPortsDropDown.value < 0)
            {
                Logger.Log.Warning($"No serial port was selected!");
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
            Logger.Log.Warning($"Serial connection lost!");
            SerialConnectionManager.Instance.CloseConnection();
            TriggerConnectionTextVisibility(true);
        }
    }
}
