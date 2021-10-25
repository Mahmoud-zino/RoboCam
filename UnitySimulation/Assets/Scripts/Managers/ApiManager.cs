using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class ApiManager : MonoBehaviour
{
    #region Singleton Setup
    public static ApiManager Instance;

    [SerializeField]
    private string url;
    [SerializeField]
    private GameObject headGo;
    [SerializeField]
    private GameObject manualControls;

    public FaceCount FaceCount { get; set; } = new FaceCount();
    public Face Face { get; set; } = new Face();
    public Camera RaspCamera { get; set; } = new Camera();
    private bool connectionState = false;

    private void Awake()
    {
        Instance = this;

        //TODO: Activate Asp.netcore web api
    }
    #endregion


    //instead of start
    private void OnEnable()
    {
        StartCoroutine(HandleGetApiData());
    }

    private IEnumerator HandleGetApiData()
    {
        StartCoroutine(GetApiData());

        yield return new WaitForSeconds(10);
        if (!connectionState)
        {
            GameObject.Find("Robot").GetComponent<AutoMovementController>().enabled = false;
            GameObject.Find("Robot").GetComponent<ManualMovementController>().enabled = true;
            GameObject.Find("GameModeDropDown").GetComponent<TMP_Dropdown>().value = 0;

            Logger.Log.Error("Can't connect to auto control api!");
            manualControls.SetActive(true);
            StopAllCoroutines();
            this.gameObject.SetActive(false);
        }
        this.connectionState = false;
        yield return HandleGetApiData();
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
            this.connectionState = true;
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
    }
}
