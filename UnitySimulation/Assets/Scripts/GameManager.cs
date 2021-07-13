using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject manualControls;
    [SerializeField]
    private GameObject apiManager;

    private GameObject apiManagerInstance;

    public void OnMenuSelectionChanged(TMP_Dropdown dropDown)
    {
        //Manuall
        if(dropDown.value == 0)
        {
            if(apiManagerInstance != null)
                Destroy(apiManagerInstance);
            manualControls.SetActive(true);
        }
        //Auto
        else
        {
            apiManagerInstance = Instantiate(apiManager, Vector3.zero, Quaternion.identity);
            manualControls.SetActive(false);
        }
    }
}
