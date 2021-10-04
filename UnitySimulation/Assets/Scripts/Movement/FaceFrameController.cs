using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FaceFrameController : MonoBehaviour
{
    private void Update()
    {
        Debug.Log(ApiManager.Instance.Face);
        Logger.Log.Information(ApiManager.Instance.Face.ToString());
    }
}
