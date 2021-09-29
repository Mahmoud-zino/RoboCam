using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Logger : MonoBehaviour
{
    [SerializeField]
    private Toggle fileLogCheckBox;

    public static Logger Instance;

    // Singleton in Unity
    private void Awake() =>
        Instance = this;

    //instead of OnDestroy
    private void OnDisable() =>
        StopAllCoroutines();

    public void Log(string message, LogType logType)
    {
        string logMessage = $"{logType.ToString()} : [{DateTime.Now.ToString()}] : \"{message}\"";
        if (fileLogCheckBox.isOn)
            LogFile(logMessage);

        LogConsole(logMessage);
    }

    public void LogFile(string message)
    {
        
    }

    public void LogConsole(string message)
    {

    }
}

public enum LogType
{
    Warning,
    Information
}

public struct Log
{
    public string Message;
    public DateTime DateTime;
    public LogType LogType;
}
