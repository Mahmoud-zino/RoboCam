using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConsoleListController : MonoBehaviour
{
    [SerializeField] private Sprite WarningIcon;
    [SerializeField] private Sprite InformationIcon;
    [SerializeField] private Sprite ErrorIcon;

    public static ConsoleListController Instance;

    [SerializeField]
    private GameObject elementTemplate;
    private GameObject g;

    private Log CallLog;
    static int mainThreadId;

    private void Awake()
    {
        Instance = this;
        mainThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
    }

    // Not Thread Safe
    private void Update()
    {
        if (CallLog.DateTime == default && CallLog.LogType == default && CallLog.Message == default)
            return;

        Log(CallLog);
        CallLog = default;
    }

    public void Log(Log log)
    {
        if(System.Threading.Thread.CurrentThread.ManagedThreadId != mainThreadId)
        {
            CallLog = log;
            return;
        }

        g = Instantiate(elementTemplate, Instance.transform);
        g.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = log.Message;        
        g.transform.GetChild(1).GetComponent<Image>().sprite = (log.LogType == LogType.Error) ? ErrorIcon :
            (log.LogType == LogType.Warning) ? WarningIcon : InformationIcon;
        g.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = $"[{log.DateTime.ToString()}]";
    }
}
