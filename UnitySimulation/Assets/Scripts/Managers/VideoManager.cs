using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class VideoManager : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject noSignalText;
    [SerializeField] private GameObject udpManagerObj;
    private Texture videoImage;
    private Coroutine streamCoroutine;

    private void Start()
    {
        videoImage = this.GetComponent<RawImage>().texture;
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
            noSignalText.SetActive(false);
            ImageConversion.LoadImage((Texture2D)videoImage, UDPManager.Instance.RecievedData);
        }
        catch
        {
            noSignalText.SetActive(true);
        }
    }
}
