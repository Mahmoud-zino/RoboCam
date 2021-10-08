using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Logger : IDisposable
{
    private static string directory = $"{System.IO.Directory.GetCurrentDirectory()}/log.txt";
    private TextWriter tw = null;
    private Log lastLog;

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
        lastLog = new Log() { Message = message, LogType = logType };

        lock (Log)
        {
            LogFile($"{logType} : [{DateTime.Now}] : \"{message}\"");
            LogConsole(message, logType);
        }
    }

    public void LogFile(string message)
    {
        tw = new StreamWriter(directory, true);
        tw.WriteLine(message);
        this.Dispose();
    }

    public void LogConsole(string message, LogType logType)
    {
        if(message.Length >= 50)
            message = $"{message.Substring(0, 50)}...";
        ConsoleListController.Log.Add(new Log() { Message = message, LogType = logType });
    }

    public void Dispose()
    {
        tw.Close();
        tw.Dispose();
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
    public LogType LogType;
}
