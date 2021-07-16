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

    private FaceCount faceCount;


    private void Awake()
    {
        //Unity singleton
        Instance = this;

        //TODO: Activate Asp.netcore web api
    }

    //instead of start
    private void OnEnable()
    {
        StartCoroutine(GetFaceCountRoutine());
    }

    //instead of OnDestroy
    private void OnDisable()
    {
        StopCoroutine(GetFaceCountRoutine());
        this.headGo.SetActive(false);
    }

    private void Update()
    {
        //if the count of the faces is 1 activate this game object
        if (faceCount != null)
            headGo.SetActive(this.faceCount.count == 1.0f);
        else
            this.headGo.SetActive(false);

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

    public IEnumerator RequestObjectRoutine(string section, UnityAction<string> callback)
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
}
