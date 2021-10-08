using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConsoleListController : MonoBehaviour
{
    [SerializeField] private Sprite warningIcon;
    [SerializeField] private Sprite informationIcon;
    [SerializeField] private Sprite errorIcon;

    public static ConsoleListController Log;

    [SerializeField]
    private GameObject elementTemplate;
    private GameObject element;

    [SerializeField] private GameObject errorPanel;
    [SerializeField] private ScrollRect scrollRect;


    private void Awake()
    {
        Log = this;
    }

    public bool resetScrollbar = false;
    private void OnGUI()
    {
        if (resetScrollbar)
        {
            resetScrollbar = false;
            SetScrollbar();
        }
    }

    public void SetScrollbar()
    {
        scrollRect.verticalNormalizedPosition = 0.0f;
    }
        
    public void Add(Log log)
    {
        if (log.LogType != LogType.Information)
            (errorPanel.GetComponent(typeof(PanelAnimManager)) as PanelAnimManager).SetPanel(true);
        if(element != null)
            element.transform.GetComponent<Outline>().effectColor = Color.black;
        element = Instantiate(elementTemplate, Log.transform);

        element.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = log.Message;
        element.transform.GetChild(1).GetComponent<Image>().sprite = 
            (log.LogType == LogType.Error) ? errorIcon :
            (log.LogType == LogType.Warning) ? warningIcon : 
            informationIcon;
        element.transform.GetComponent<Outline>().effectColor = Color.red;
        element.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "1";
        element.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = $"[{DateTime.Now}]";
        if (scrollRect.verticalNormalizedPosition <= 0.01f || scrollRect.content.childCount <= 9)
            resetScrollbar = true;
    }

    public void Add()
    {
        element.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = 
            (int.Parse(element.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text) + 1).ToString();
        element.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = $"[{DateTime.Now}]";
    }
}
