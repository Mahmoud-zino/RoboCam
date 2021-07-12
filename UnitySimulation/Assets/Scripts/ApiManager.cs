using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class ApiManager : MonoBehaviour
{
    [SerializeField]
    private string url;
    [SerializeField]
    private GameObject headGo;

    private Camera raspCamera = new Camera();
    private Face face = new Face();
    private FaceCount faceCount = new FaceCount();

    private void Awake()
    {
        //TODO: Activate Asp.netcore web api
    }

    private void Start()
    {
        StartCoroutine(RequestObjectRoutine("Camera", (value) =>
        {
            this.raspCamera = JsonUtility.FromJson<Camera>(value);
        }));
        StartCoroutine(GetFaceCountRoutine());
    }

    private void Update()
    {
        //if the count of the faces is 1 activate this game object
        headGo.SetActive(this.faceCount.count == 1);
    }

    private IEnumerator GetFaceCountRoutine()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        StartCoroutine(RequestObjectRoutine("FaceCount", (value) =>
        {
            this.faceCount = JsonUtility.FromJson<FaceCount>(value);
            //making sure the first routine has ended
            StartCoroutine(GetFaceCountRoutine());
        }));
    }

    private IEnumerator RequestObjectRoutine(string section, UnityAction<string> callback)
    {
        UnityWebRequest req = UnityWebRequest.Get($"{url}/{section}");
        yield return req.SendWebRequest();

        if(req.result == UnityWebRequest.Result.Success)
        {
            callback(req.downloadHandler.text);
        }
        else
        {
            Debug.LogError($"{req.result} Error:\n{req.error}");
            yield return new WaitForSecondsRealtime(1);
            StartCoroutine(RequestObjectRoutine(section, callback));
        }
    }
}
