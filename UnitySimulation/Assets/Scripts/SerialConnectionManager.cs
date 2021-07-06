using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO.Ports;
using System;
using System.Threading;
using System.Collections;

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

    public void Connect(string portName, int baudRate = 9600, Parity partiy = Parity.None, int dataBits = 8, StopBits stopBits = StopBits.One)
    {
        try
        {
            this.serialPort = new SerialPort(portName, baudRate, partiy, dataBits, stopBits);
            StartThread();
        }
        catch (Exception)
        {
            Debug.LogError("Failed Create and start Thread pool");
        }
    }

    public bool IsConnected()
    {
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

    private void StartThread()
    {
        outputQueue = Queue.Synchronized(new Queue());
        inputQueue = Queue.Synchronized(new Queue());

        thread = new Thread(ThreadLoop);
        thread.Start();
    }

    #region MultiThread Setup & actions
    private void WriteSerialMessage(string message)
    {
        if (!this.serialPort.IsOpen)
        {
            Debug.LogError("Trying to send serial message with a closed serial Connection");
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
        this.serialPort.Open();

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
            isLooping = false;
    }

    public void Dispose()
    {
        CloseConnection();
    }
    #endregion
}
