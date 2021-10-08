using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public enum LogType
{
    Information,
    Warning,
    Error
}

public class Log
{
    public Log(string message, LogType logType)
    {
        this.Message = message;
        this.LogType = logType;
    }

    public string Message { get; private set; }
    public LogType LogType { get; private set; }

    public override bool Equals(object obj)
    {
        Log compareLog = (obj as Log);
        return compareLog.Message == this.Message && compareLog.LogType == this.LogType;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}

public class Logger
{
    private static readonly string directory = $"{Directory.GetCurrentDirectory()}/log.txt";
    private Log lastLog;

    #region SingletonSetup
    private static Logger instance;
    public static Logger Log
    {
        get
        {
            if (instance == null)
            {
                instance = new Logger();
                File.Delete(directory);
            }
            return instance;
        }
    }
    private Logger() { }
    #endregion

    public void Information(string message)
    {
        ExecuteLog(message, LogType.Information);
    }

    public void Error(string message)
    {
        ExecuteLog(message, LogType.Error);
    }

    public void Warning(string message)
    {
        ExecuteLog(message, LogType.Warning);
    }

    public void ExecuteLog(string message, LogType logType)
    {
        if (lastLog != null && lastLog.Equals(new Log(message, logType)))
            ConsoleListController.Instance.Add();
        else
        {
            LogFile(message, logType);
            LogConsole(message, logType);
        }

        lastLog = new Log(message, logType);
    }

    public void LogFile(string message, LogType logType)
    {
        message = $"{logType} : [{DateTime.Now}] : \"{message}\"";
        using (TextWriter textWriter = new StreamWriter(directory, true))
        {
            textWriter.WriteLine(message);
            textWriter.Close();
        }
    }

    public void LogConsole(string message, LogType logType)
    {
        if(message.Length >= 50)
            message = $"{message.Substring(0, 50)}...";
        ConsoleListController.Instance.Add(new Log(message, logType));
    }
}