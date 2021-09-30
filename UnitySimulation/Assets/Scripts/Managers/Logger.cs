using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Logger
{
    [SerializeField]
    private Toggle fileLogCheckBox;

    private static Logger instance;
    public static Logger Instance
    {
        get
        {
            if (instance == null)
                instance = new Logger();
            return instance;
        }
    }
    private Logger() { }

    public void Log(string message, LogType logType)
    {
        LogFile($"{logType.ToString()} : [{DateTime.Now.ToString()}] : \"{message}\"");
        LogConsole(message, logType);
    }

    public void LogFile(string message)
    {
        //TODO: File write
    }

    public void LogConsole(string message, LogType logType)
    {
        if(message.Length >= 50)
            message = message.Substring(0, 50) + "...";
        ConsoleListController.Instance.Log(new Log() { Message = message, DateTime = DateTime.Now, LogType = logType });
    }
}

public enum LogType
{
    Warning,    // Effected nicht
    Information,   // Info
    Error // Critical
}

public struct Log
{
    public string Message;
    public DateTime DateTime;
    public LogType LogType;
}
