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

    public void TriggerConnection(TMP_Text connect, TMP_Text disconnect, TMP_Dropdown dropdown)
    {
        //Connect
        if(connect.gameObject.activeSelf)
        {
            //No port was selected
            if(dropdown.value < 0)
            {
                Debug.LogError("No port was selected!");
                return;
            }   
            
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

    public void SendMoveMessage(int[] positions)
    {
        string message = "[";
        for (int i = 0; i < positions.Length; i++)
        {
            message += positions[i].ToString("000");
            message += "][";
        }
        message = message.Remove(message.Length - 1);
        this.serialPort.Write(message);
    }

    public void SendResetMessage()
    {
        this.serialPort.Write("R");
    }

    public string GetCurrentPostion()
    {
        this.serialPort.Write("G");
        return this.serialPort.ReadLine();
    }

    private void OnApplicationQuit()
    {
        if (this.serialPort != null && this.serialPort.IsOpen)
            this.serialPort.Close();
    }
}
