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
    [SerializeField] private ScrollRect scrollRect;

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
            Log log = LogQueue.Dequeue();

            if (Lastg != null && log.Message == Lastg.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text)
            {
                g.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = (int.Parse(g.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text) + 1).ToString();
                g.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = $"[{log.DateTime.ToString()}]";
                return;
            }

            g = Instantiate(elementTemplate, Instance.transform);
            if (Lastg == null)
                Lastg = g;

            if (log.LogType != LogType.Information)
                (ErrorPanel.GetComponent(typeof(PanelAnimManager)) as PanelAnimManager).SetPanel(true);

            g.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = log.Message;
            g.transform.GetChild(1).GetComponent<Image>().sprite = (log.LogType == LogType.Error) ? ErrorIcon :
                (log.LogType == LogType.Warning) ? WarningIcon : InformationIcon;
            g.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = $"[{log.DateTime.ToString()}]";
            g.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "1";
            Lastg.transform.GetComponent<Outline>().effectColor = Color.black;
            g.transform.GetComponent<Outline>().effectColor = Color.red;

            Lastg = g;
        }
    }

    public void SetScrollbar() =>
        scrollRect.verticalNormalizedPosition = 0.0f;

    public void Log(Log log) =>
        LogQueue.Enqueue(log);
}
