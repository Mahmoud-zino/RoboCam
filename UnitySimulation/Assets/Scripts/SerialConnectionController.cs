using System.Collections.Generic;
using UnityEngine;
using System.Linq;
//its displaying an Error in editor but the error doesn't exist
using System.IO.Ports;

public class SerialConnectionController : MonoBehaviour
{
    #region Singleton Setup
    public static SerialConnectionController Instance;
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        foreach (var item in GetAvailablePorts())
        {
            Debug.Log(item);
        }
    }
    #endregion

    public List<string> GetAvailablePorts()
    {
        return SerialPort.GetPortNames().ToList();
    }
}
