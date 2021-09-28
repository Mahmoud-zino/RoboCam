using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ErrorListController : MonoBehaviour
{
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

    private List<Log> Logs { get; set; } = new List<Log>();

    private void Start()
    {
        GameObject buttonTemplate = transform.GetChild(0).gameObject;
        GameObject g;

        Logs.Add(new Log() { Message = "1", DateTime = DateTime.Now, LogType = LogType.Information });
        Logs.Add(new Log() { Message = "2", DateTime = DateTime.Now, LogType = LogType.Information });
        Logs.Add(new Log() { Message = "3", DateTime = DateTime.Now, LogType = LogType.Information });

        for (int i = 0; i < Logs.Count; i++)
        {
            g = Instantiate(buttonTemplate, transform);
            g.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Logs[i].Message;
            g.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = Logs[i].DateTime.ToString();
            g.transform.GetChild(2).GetComponent<Image>().sprite = (Logs[i].LogType == LogType.Information) ?
                Sprite.Create(new Texture2D(10, 10), new Rect(), new Vector2()) :
                Sprite.Create(new Texture2D(10, 10), new Rect(), new Vector2());
        }

        Destroy(buttonTemplate);

        //Thread.Sleep(5000);

        //while (Logs.Count != 0) { Destroy(transform.GetChild(0).gameObject); }
    }

    private void UpdateList()
    {
    }

    private void ClearList()
    {


    }
}
