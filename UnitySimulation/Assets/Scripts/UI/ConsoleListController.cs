using System;
using System.Collections.Concurrent;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConsoleListController : MonoBehaviour
{
    [SerializeField] private Sprite warningIcon;
    [SerializeField] private Sprite informationIcon;
    [SerializeField] private Sprite errorIcon;

    [SerializeField] private PanelAnimManager panelAnimManager;
    [SerializeField] private ScrollRect scrollRect;

    [SerializeField] private GameObject elementTemplate;
    private GameObject lastElement;


    public static ConsoleListController Instance;
    public readonly ConcurrentQueue<Action> RunOnMainThread = new ConcurrentQueue<Action>();


    private void Awake()
    {
        Instance = this;
    }

    public bool resetScrollbar = false;
    private void OnGUI()
    {
        if (resetScrollbar)
        {
            resetScrollbar = false;
            scrollRect.verticalNormalizedPosition = 0.0f;
        }
    }

    void Update()
    {
        if (!RunOnMainThread.IsEmpty)
        {
            while (RunOnMainThread.TryDequeue(out var action))
            {
                action?.Invoke();
            }
        }
    }

    public void Add(Log log)
    {
        //Opens panel on warning/error messages
        if (log.LogType != LogType.Information)
            panelAnimManager.SetPanel(true);

        if(lastElement != null)
            lastElement.transform.GetComponent<Outline>().effectColor = Color.black;

        lastElement = Instantiate(elementTemplate, this.transform);

        lastElement.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = log.Message;
        lastElement.transform.GetChild(1).GetComponent<Image>().sprite = 
            (log.LogType == LogType.Error) ? errorIcon :
            (log.LogType == LogType.Warning) ? warningIcon : 
            informationIcon;

        lastElement.transform.GetComponent<Outline>().effectColor = Color.red;

        lastElement.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "1";
        lastElement.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = $"[{DateTime.Now}]";

        if (scrollRect.verticalNormalizedPosition <= 0.01f || scrollRect.content.childCount <= 9)
            resetScrollbar = true;
    }

    public void Add()
    {
        TextMeshProUGUI occurrenceCounter = lastElement.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        occurrenceCounter.text = $"{int.Parse(occurrenceCounter.text) + 1}";

        lastElement.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = $"[{DateTime.Now}]";
    }
}
