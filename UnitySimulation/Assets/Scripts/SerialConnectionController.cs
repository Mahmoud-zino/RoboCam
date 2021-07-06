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
                RobotManualMovementController roboScript = GameObject.Find("Robot").GetComponent<RobotManualMovementController>();
                //roboScript.SetValuesToMotors(ConvertStringMessageToRobotValues(GetCurrentPostion()));
                Debug.Log(GetCurrentPostion());
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
        if (this.serialPort == null || this.serialPort.IsOpen)
            return;

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
        if (this.serialPort == null || this.serialPort.IsOpen)
            return;
        this.serialPort.Write("R");
    }

    public string GetCurrentPostion()
    {
        if (this.serialPort == null || !this.serialPort.IsOpen)
            return "";
        this.serialPort.Write("G");
        //Debug.Log("Writed G Serially");
        return this.serialPort.ReadLine();
    }

    public int[] ConvertStringMessageToRobotValues(string message)
    {
        if (message.Length < 19)
        {
            Debug.LogError("String message is too short");
            return null;
        }

        int[] values = new int[4];

        int messageIndex = 1;

        for (int i = 0; i < 4; i++)
        {
            string stringVal = $"{message[messageIndex]}{message[messageIndex + 1]}{message[messageIndex + 2]}";
            if(!int.TryParse(stringVal, out values[i]))
            {
                Debug.LogError("Convertion Error");
                return null;
            }
            messageIndex += 5;
        }
        return values;
    }

    private void OnApplicationQuit()
    {
        if (this.serialPort != null && this.serialPort.IsOpen)
            this.serialPort.Close();
    }
}
