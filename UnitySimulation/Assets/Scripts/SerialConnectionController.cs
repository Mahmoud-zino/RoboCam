using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO.Ports;

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
}
