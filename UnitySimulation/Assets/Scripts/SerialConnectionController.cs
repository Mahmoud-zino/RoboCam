using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO.Ports;
using TMPro;

public class SerialConnectionController : MonoBehaviour
{
    private SerialPort serialPort;

    #region Singleton Setup
    public static SerialConnectionController Instance;
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    #endregion

    public List<string> GetAvailablePorts()
    {
        return SerialPort.GetPortNames().ToList();
    }

    public void TriggerConnection(TextMeshPro connect, TextMeshPro disconnect, TMP_Dropdown dropdown)
    {
        //Connect
        if(connect.gameObject.activeSelf)
        {
            this.serialPort = new SerialPort(dropdown.options[dropdown.value].text, 9600, Parity.None, 8, StopBits.One);
            this.serialPort.Open();
            if(this.serialPort.IsOpen)
            {
                connect.gameObject.SetActive(false);
                disconnect.gameObject.SetActive(true);
            }
            //Error opening serial connection
            else
            {
                Debug.LogError("Error Opening the port");
            }
        }
        //Disconnect
        else
        {
            this.serialPort.Close();
            connect.gameObject.SetActive(true);
            disconnect.gameObject.SetActive(false);
        }
    }

    private void OnApplicationQuit()
    {
        if (this.serialPort != null && this.serialPort.IsOpen)
            this.serialPort.Close();
    }
}
