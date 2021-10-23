using System.Diagnostics;
using System.IO;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject manualControls;
    [SerializeField] private GameObject apiManager;
    [SerializeField] private GameObject aboutScreen;

    public void OnMenuSelectionChanged(TMP_Dropdown dropDown)
    {
        //Manuall
        if(dropDown.value == 0)
        {
            manualControls.SetActive(true);
            apiManager.SetActive(false);
            GameObject.Find("Robot").GetComponent<AutoMovementController>().enabled = false;
            GameObject.Find("Robot").GetComponent<ManualMovementController>().enabled = true;
        }
        //Auto
        else
        {
            manualControls.SetActive(false);
            apiManager.SetActive(true);
            GameObject.Find("Robot").GetComponent<AutoMovementController>().enabled = true;
            GameObject.Find("Robot").GetComponent<ManualMovementController>().enabled = false;
        }
    }


    public void OnBtnExitClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void OnBtnAboutClick()
    {
        this.aboutScreen.SetActive(true);
    }

    public void OnBtnAboutOkClick()
    {
        this.aboutScreen.SetActive(false);
    }

    public void OnBtnRestartAPIClick()
    {
        string apiExeDirectory = $"{Directory.GetCurrentDirectory()}";
        //Process.Start("C:\\");
    }
}
