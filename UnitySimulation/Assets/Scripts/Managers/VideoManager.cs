using System.Collections;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class VideoManager : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject udpManagerObj;
    [SerializeField] private Texture2D nosignalImage;
    private RawImage videoImage;
    private Coroutine streamCoroutine;
    private Texture2D tex;


    private const string SS_FOLDER_NAME = "Pictures";
    public string ScreenShotFolderPath { get; set; }

    private void Start()
    {
        videoImage = this.GetComponent<RawImage>();
        tex = new Texture2D(640, 480);


        ScreenShotFolderPath = Directory.GetCurrentDirectory();

        ScreenShotFolderPath = Path.Combine(ScreenShotFolderPath, SS_FOLDER_NAME);

        if (!Directory.Exists(ScreenShotFolderPath))
            Directory.CreateDirectory(ScreenShotFolderPath);
    }

    public void TriggerVideo()
    {
        if (animator.GetBool("Open"))
        {
            udpManagerObj.SetActive(true);
            streamCoroutine = StartCoroutine(StreamRoutine());
        }
        else
        {
            udpManagerObj.SetActive(false);
            StopCoroutine(streamCoroutine);
        }
    }

    private IEnumerator StreamRoutine()
    {
        yield return new WaitForEndOfFrame();
        try
        {
            ImageConversion.LoadImage(tex, UDPManager.Instance.RecievedData);
            if (UDPManager.Instance.RecievingError || tex.width <= 8 || tex.height <= 8)
                throw new System.Exception();
            tex.Apply();
            videoImage.texture = tex;
        }
        catch
        {
            videoImage.texture = nosignalImage;
        }
        streamCoroutine = StartCoroutine(StreamRoutine());
    }

    public void OnExplorerClick()
    {
        Process.Start("explorer.exe", this.ScreenShotFolderPath);
    }
}
