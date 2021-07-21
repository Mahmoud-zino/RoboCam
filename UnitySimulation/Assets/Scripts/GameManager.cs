using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject manualControls;
    [SerializeField]
    private GameObject apiManager;
    [SerializeField]
    private GameObject videoManager;

    public void OnMenuSelectionChanged(TMP_Dropdown dropDown)
    {
        //Manuall
        if(dropDown.value == 0)
        {
            manualControls.SetActive(true);
            apiManager.SetActive(false);
        }
        //Auto
        else
        {
            manualControls.SetActive(false);
            apiManager.SetActive(true);
        }
    }

    public void TriggerVideoVisibility(TextMeshProUGUI btnText)
    {
        this.videoManager.SetActive(!this.videoManager.activeSelf);
        btnText.text = this.videoManager.activeSelf ? "Hide Video" : "Show Video";
    }
}
