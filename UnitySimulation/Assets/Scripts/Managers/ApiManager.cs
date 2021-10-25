using System.Collections;
using TMPro;
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
    }
    #endregion

    [SerializeField] private GameObject manualControls;

    public FaceCount FaceCount { get; set; } = new FaceCount();
    public Face Face { get; set; } = new Face();
    public Camera RaspCamera { get; set; } = new Camera();

    private string url = "http://192.168.2.10:5000/api";
    private bool connectedToAPI;

    private Coroutine getApiDataCoroutine;
    private Coroutine handleGetApiDataCoroutine;

    //instead of start
    private void OnEnable()
    {
        getApiDataCoroutine = StartCoroutine(GetApiData());
        handleGetApiDataCoroutine = StartCoroutine(HandleGetApiData());
    }

    private IEnumerator HandleGetApiData()
    {
        yield return new WaitForSeconds(10);
        if (this.connectedToAPI)
        {
            this.connectedToAPI = false;
            handleGetApiDataCoroutine = StartCoroutine(HandleGetApiData());
        }
        else
        {
            GameObject robotGO = GameObject.Find("Robot");
            robotGO.GetComponent<AutoMovementController>().enabled = false;
            robotGO.GetComponent<ManualMovementController>().enabled = true;

            GameObject.Find("GameModeDropDown").GetComponent<TMP_Dropdown>().value = 0;

            Logger.Log.Error("Can't connect to auto control api!");

            manualControls.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        StopCoroutine(this.getApiDataCoroutine);
        StopCoroutine(this.handleGetApiDataCoroutine);
    }

    private IEnumerator GetApiData()
    {
        StartCoroutine(RequestObjectRoutine(nameof(Camera), (value) =>
        {
            this.RaspCamera = JsonUtility.FromJson<Camera>(value);
            this.connectedToAPI = true;
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
