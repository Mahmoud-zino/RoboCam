using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class ApiManager : MonoBehaviour
{
    #region Singleton Setup
    public static ApiManager Instance;

    private void Awake()
    {
        Instance = this;

        //TODO: Activate Asp.netcore web api
    }
    #endregion

    private string url = "http://192.168.2.10:5000/api";

    public FaceCount FaceCount { get; set; } = new FaceCount();
    public Face Face { get; set; } = new Face();
    public Camera RaspCamera { get; set; } = new Camera();


    //instead of start
    private void OnEnable()
    {
        StartCoroutine(GetApiData());
    }

    //instead of OnDestroy
    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator GetApiData()
    {
        StartCoroutine(RequestObjectRoutine(nameof(Camera), (value) =>
        {
            this.RaspCamera = JsonUtility.FromJson<Camera>(value);
        }));

        StartCoroutine(RequestObjectRoutine(nameof(FaceCount), (value) =>
        {
            this.FaceCount = JsonUtility.FromJson<FaceCount>(value);
            if(this.FaceCount?.faceCount == 1)
            {
                StartCoroutine(RequestObjectRoutine(nameof(Face), (value) =>
                {
                    this.Face = JsonUtility.FromJson<Face>(value);
                }));
            }
        }));

        yield return new WaitForSecondsRealtime(0.05f);
        yield return GetApiData();
    }


    private IEnumerator RequestObjectRoutine(string section, UnityAction<string> callback)
    {
        UnityWebRequest req = UnityWebRequest.Get($"{url}/{section}");
        yield return req.SendWebRequest();

        if(req.result == UnityWebRequest.Result.Success)
            callback(req.downloadHandler.text);
        else
            Debug.Log($"{req.result}: {req.error}\nurl was: {url}/{section}");
    }
}
