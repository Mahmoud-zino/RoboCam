using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class VideoManager : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject udpManagerObj;
    private RawImage videoImage;
    private Coroutine streamCoroutine;
    [SerializeField] private Texture2D nosignalImage;

    private Texture2D tex;

    private void Start()
    {
        videoImage = this.GetComponent<RawImage>();
        tex = new Texture2D(640, 480);
    }

    public void TriggerVideo()
    {
        if (animator.GetBool("IsPanelOpen"))
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
            if (UDPManager.Instance.RecievingError && tex.width <= 8 && tex.height <= 8)
                throw new System.Exception();
            tex.Apply();
            videoImage.texture = tex;
        }
        catch
        {
            videoImage.texture = nosignalImage;
        }
        yield return StreamRoutine();
    }
}
