using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using UnityEngine;

//Serial Connection is multi threaded to avoid freezing the gui
public sealed class SerialConnectionManager : IDisposable
{
    private SerialPort serialPort;
    private Thread thread;
    private Queue outputQueue;
    private Queue inputQueue;
    private bool isLooping = true;

    #region Singleton Setup
    private static SerialConnectionManager instance;
    public static SerialConnectionManager Instance
    {
        get
        {
            if (instance == null)
                instance = new SerialConnectionManager();
            return instance;
        }
    }
    private SerialConnectionManager() { }
    #endregion

    public List<string> GetAvailablePorts()
    {
        return SerialPort.GetPortNames().ToList();
    }

    public void Connect(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
    {
        try
        {
            this.serialPort = new SerialPort(portName, baudRate, parity, dataBits, stopBits);
            StartThread();
        }
        catch (Exception)
        {
            Debug.LogError("Failed Create and start Thread pool");
            Logger.Log.Error("Failed Create and start Thread pool");
        }
    }

    public bool IsConnected()
    {
        if (this.serialPort is null)
            return false;
        return this.serialPort.IsOpen;
    }

    public void SendSerialMessage(string message)
    {
        outputQueue.Enqueue(message);
    }

    public string RecieveSerialMessage()
    {
        if (inputQueue.Count == 0)
            return null;

        return inputQueue.Dequeue() as string;
    }

    public void FlushData()
    {
        inputQueue.Clear();
        outputQueue.Clear();
    }

    #region MultiThread Setup & actions
    private void StartThread()
    {
        outputQueue = Queue.Synchronized(new Queue());
        inputQueue = Queue.Synchronized(new Queue());
        this.isLooping = true;
        thread = new Thread(ThreadLoop);
        thread.Start();
    }

    private void WriteSerialMessage(string message)
    {
        if (!this.serialPort.IsOpen)
        {
            Debug.LogError("Trying to send serial message with a closed serial Connection");
            Logger.Log.Warning("Serial connection closed!");
            return;
        }

        this.serialPort.WriteLine(message);

        this.serialPort.BaseStream.Flush();
    }

    private string ReadSerialMessage()
    {
        if (!this.serialPort.IsOpen)
        {
            Debug.LogError("Trying to read serial message with a closed serial Connection");
            Logger.Log.Warning("Serial connection closed!");
            return null;
        }

        try
        {
            return this.serialPort.ReadLine();
        }
        catch (TimeoutException)
        {
            return null;
        }
        //Connection lost
        catch(System.IO.IOException)
        {
            this.CloseConnection();
            return null;
        }
    }

    private void ThreadLoop()
    {
        this.serialPort.ReadTimeout = 50;
        try
        {
            this.serialPort.Open();
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            Logger.Log.Warning("Failed to open serial connection!");
        }

        while (IsLooping())
        {
            //Send Serially
            if(outputQueue.Count != 0)
            {
                string message = outputQueue.Dequeue() as string;
                WriteSerialMessage(message);
            }

            //read serially
            string result = ReadSerialMessage();
            if (result != null)
                inputQueue.Enqueue(result);
        }

        this.serialPort.Close();
    }

    private bool IsLooping()
    {
        lock (this)
            return this.isLooping;
    }

    public void CloseConnection()
    {
        lock (this)
            this.isLooping = false;
    }

    public void Dispose()
    {
        CloseConnection();
    }
    #endregion
}
