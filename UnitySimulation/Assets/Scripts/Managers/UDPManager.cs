using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

public class UDPManager : MonoBehaviour
{
    public static UDPManager Instance;
    [HideInInspector]
    public byte[] RecievedData;

    [SerializeField]
    private int udpPort = 8000;
    private Thread readThread;
    private UdpClient udpClient;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        readThread = new Thread(new ThreadStart(RecieveData))
        {
            IsBackground = true
        };
        readThread.Start();
    }

    private void RecieveData()
    {
        udpClient = new UdpClient(udpPort); 
        udpClient.Client.ReceiveTimeout = 1000;

        while (true)
        {
            try
            {
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                RecievedData = udpClient.Receive(ref anyIP);
            }
            catch (Exception e)
            {
                Debug.Log(e);
                Logger.Instance.Log(e.Message, LogType.Error);
            }
        }
    }

    private void OnDisable()
    {
        if (readThread.IsAlive)
            readThread.Abort();

        if (udpClient != null)
            udpClient.Close();
    }

    private void OnApplicationQuit()
    {
        if (readThread.IsAlive)
            readThread.Abort();

        if (udpClient != null)
            udpClient.Close();
    }
}
