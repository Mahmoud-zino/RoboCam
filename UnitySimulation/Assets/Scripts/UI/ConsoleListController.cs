using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConsoleListController : MonoBehaviour
{
    [SerializeField]
    GameObject elementTemplate;
    GameObject g;

    public static ConsoleListController Instance;

    // Singleton in Unity
    private void Awake() =>
        Instance = this;

    private List<Log> Logs = new List<Log>();

    public void Log(Log log)
    {
        Logs.Add(log);

        g = Instantiate(elementTemplate, transform);
        g.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = log.Message;        
    }

    private void Start()
    {
        Log(new Log() { Message = "3", DateTime = DateTime.Now, LogType = LogType.Information });
        Log(new Log() { Message = "3", DateTime = DateTime.Now, LogType = LogType.Information });
        Log(new Log() { Message = "3", DateTime = DateTime.Now, LogType = LogType.Information });
        Log(new Log() { Message = "3", DateTime = DateTime.Now, LogType = LogType.Information });
        Log(new Log() { Message = "3", DateTime = DateTime.Now, LogType = LogType.Information });
        Log(new Log() { Message = "3", DateTime = DateTime.Now, LogType = LogType.Information });
    }
}
