using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class ApiManager : MonoBehaviour
{
    public static ApiManager Instance;

    [SerializeField]
    private string url;
    [SerializeField]
    private GameObject headGo;

    public FaceCount FaceCount { get; set; } = new FaceCount();
    public Face Face { get; set; } = new Face();
    public Camera RaspCamera { get; set; } = new Camera();

    private void Awake()
    {
        //Unity singleton
        Instance = this;

        //TODO: Activate Asp.netcore web api
    }


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
        StartCoroutine(RequestObjectRoutine("Camera", (value) =>
        {
            this.RaspCamera = JsonUtility.FromJson<Camera>(value);
        }));

        StartCoroutine(RequestObjectRoutine("FaceCount", (value) =>
        {
            this.FaceCount = JsonUtility.FromJson<FaceCount>(value);
            if(this.FaceCount?.faceCount == 1)
            {
                StartCoroutine(RequestObjectRoutine("Face", (value) =>
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
        {
            callback(req.downloadHandler.text);
        }
        else
        {
            // TODO: log only one time in file
        }
    }
}
