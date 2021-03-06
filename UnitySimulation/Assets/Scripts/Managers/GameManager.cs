using TMPro;
using UnityEngine;

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

    public void TriggerVideoVisibility(TextMeshProUGUI btnText)
    {
        this.videoManager.SetActive(!this.videoManager.activeSelf);
        btnText.text = this.videoManager.activeSelf ? "Hide Video" : "Show Video";
    }
}
