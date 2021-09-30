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
    private GameObject Lastg;

    [SerializeField] private GameObject ErrorPanel;


    private Queue<Log> LogQueue { get; set; } = new Queue<Log>();

    private void Awake()
    {
        Instance = this;
    }

    // Not Thread Safe
    private void Update()
    {
        for (int i = 0; i < LogQueue.Count; i++)
        {
            g = Instantiate(elementTemplate, Instance.transform);

            if (Lastg == null)
                Lastg = g;

            Log log = LogQueue.Dequeue();
            if (log.LogType != LogType.Information)
                (ErrorPanel.GetComponent(typeof(LeftAnimManager)) as LeftAnimManager).SetPanel(true);

            g.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = log.Message;
            g.transform.GetChild(1).GetComponent<Image>().sprite = (log.LogType == LogType.Error) ? ErrorIcon :
                (log.LogType == LogType.Warning) ? WarningIcon : InformationIcon;
            g.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = $"[{log.DateTime.ToString()}]";
            ColorUtility.TryParseHtmlString("#575D5E", out Color grey);
            ColorUtility.TryParseHtmlString("#808080", out Color lightGrey);
            Lastg.transform.GetComponent<Image>().color = grey;
            g.transform.GetComponent<Image>().color = grey;
            Lastg.transform.GetComponent<Outline>().effectColor = Color.black;
            g.transform.GetComponent<Outline>().effectColor = Color.red;

            Lastg = g;
        }
    }

    public void Log(Log log) =>
        LogQueue.Enqueue(log);
}
