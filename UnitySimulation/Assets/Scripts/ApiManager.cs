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
    private GameObject headGO;
    
    private GameObject roboCam;


    private Camera raspCamera = new Camera();
    private Face face = new Face();
    private FaceCount faceCount;

    private void Awake()
    {
        //TODO: Activate Asp.netcore web api
    }

    private void Start()
    {
        roboCam = GameObject.Find("CamBody");
        headGO = GameObject.Find("MortyHead");

        StartCoroutine(RequestObjectRoutine("Camera", (value) =>
        {
            this.raspCamera = JsonUtility.FromJson<Camera>(value);
        }));
        StartCoroutine(GetFaceCountRoutine());
    }

    private void Update()
    {
        if(faceCount != null)
            //if the count of the faces is 1 activate this game object
        {
            this.headGO.SetActive(this.faceCount.count == 1.0f);
            StartCoroutine(GetFacePositionRoutine());
            SetHeadPosition();
        }
        else
            this.headGO.SetActive(false);

    }

    private void SetHeadPosition()
    {
        this.headGO.gameObject.transform.position = new Vector3(this.roboCam.transform.position.x, this.roboCam.transform.position.y, this.roboCam.transform.position.z + 20);
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

    private IEnumerator GetFacePositionRoutine()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        StartCoroutine(RequestObjectRoutine("Face", (value) =>
        {
            this.face = JsonUtility.FromJson<Face>(value);
            //making sure the first routine has ended
            StartCoroutine(GetFacePositionRoutine());
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
            Debug.LogError($"{req.result}: {req.error}\nurl was: {url}/{section}");
            yield return new WaitForSecondsRealtime(1);
            StartCoroutine(RequestObjectRoutine(section, callback));
        }
    }

    private void OnDestroy()
    {
        headGO.SetActive(false);
    }
}
