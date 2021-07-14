using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject manualControls;
    [SerializeField]
    private GameObject apiManager;

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
}
