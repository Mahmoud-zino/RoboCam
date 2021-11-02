using System.Diagnostics;
using System.IO;
using TMPro;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject manualControls;
    [SerializeField] private GameObject apiManager;
    [SerializeField] private GameObject aboutScreen;
    
    private string apiExeDirectory;

    Process apiProcess = null;

    GameObject robotGO;

    private void Start()
    {
        robotGO = GameObject.Find("Robot");
        apiExeDirectory = $"{Application.streamingAssetsPath}/API/RoboCamApi.exe";
        OnBtnRestartAPIClick();
    }
    
    private void OnDisable()
    {
        try { this.apiProcess.Kill(); }
        catch { }
    }
    
    public void OnMenuSelectionChanged(TMP_Dropdown dropDown)
    {
        manualControls.SetActive(dropDown.value == 0);
        apiManager.SetActive(dropDown.value != 0);
        robotGO.GetComponent<AutoMovementController>().enabled = (dropDown.value != 0);
        robotGO.GetComponent<ManualMovementController>().enabled = (dropDown.value == 0);
    }


    public void OnBtnExitClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void OnBtnAboutClick(bool active)
    {
        this.aboutScreen.SetActive(active);
    }

    public void OnBtnRestartAPIClick()
    {
        if (!File.Exists(apiExeDirectory))
        {
            Logger.Log.Error("Api executable path not defined!");
            return;
        }

        if (this.apiProcess is null)
            this.apiProcess = Process.Start(apiExeDirectory);
        else
        {
            try 
            { 
                this.apiProcess.Kill();
            }
            catch 
            {
            }

            this.apiProcess = Process.Start(apiExeDirectory);
        }
    }
}
