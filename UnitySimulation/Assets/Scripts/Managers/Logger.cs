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
    public static Logger Log
    {
        get
        {
            if (instance == null)
                instance = new Logger();
            return instance;
        }
    }
    private Logger() { }

    public void Information(string message) => ExecuteLog(message, LogType.Information);

    public void Error(string message) => ExecuteLog(message, LogType.Error);

    public void Warning(string message) => ExecuteLog(message, LogType.Warning);

    public void ExecuteLog(string message, LogType logType)
    {
        lock (Log)
        {
            LogFile($"{logType.ToString()} : [{DateTime.Now.ToString()}] : \"{message}\"");
            LogConsole(message, logType);
        }
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
    Warning,
    Information,
    Error
}

public struct Log
{
    public string Message;
    public DateTime DateTime;
    public LogType LogType;
}
