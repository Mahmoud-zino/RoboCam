using System;
using System.IO;
using TMPro;
using UnityEngine;

public class ScreenShotManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI screenshotTimerText;
    [SerializeField] private Animator screenShotAnim;
    [SerializeField] private AutoMovementController autoMovement;
    [SerializeField] private GameObject btnScreenShot;
    [SerializeField] private VideoManager videoManager;

    private float screenshotTimer = 4.0f;

    private void Update()
    {
        if (autoMovement.isActiveAndEnabled)
        {
            AutoScreenShotCapture();
            btnScreenShot.SetActive(false);
        }
        else
            btnScreenShot.SetActive(true);
    }

    private void TakeScreenShot()
    {
        string imageName = $"Picture_{DateTime.Now.ToString("ssmmHHddMMyyyy")}.jpg";
        File.WriteAllBytes(Path.Combine(videoManager.ScreenShotFolderPath, imageName), UDPManager.Instance.RecievedData);
    }

    private void AutoScreenShotCapture()
    {
        if (ApiManager.Instance?.FaceCount == null || ApiManager.Instance.FaceCount.faceCount != 1)
        {
            this.screenshotTimerText.gameObject.SetActive(false);
            return;
        }

        if (autoMovement.IsRobotAtPosition)
        {
            screenshotTimer -= Time.deltaTime;
            if (screenshotTimer < 1)
            {
                screenshotTimer = 4.0f;
                this.screenshotTimerText.gameObject.SetActive(false);
                screenShotAnim.SetTrigger("ScreenShot");
                this.TakeScreenShot();
                return;
            }
            else
                this.screenshotTimerText.gameObject.SetActive(true);
        }
        else
        {
            screenshotTimer = 4.0f;
            this.screenshotTimerText.gameObject.SetActive(false);
        }
        this.screenshotTimerText.text = ((int)screenshotTimer).ToString();
    }

    public void OnScreenShotClick()
    {
        if (UDPManager.Instance.RecievingError)
            return;

        screenShotAnim.SetTrigger("ScreenShot");
        TakeScreenShot();
    }
}