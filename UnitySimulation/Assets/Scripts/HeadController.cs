using System.Collections;
using UnityEngine;

public class HeadController : MonoBehaviour
{
    private Face face = new Face();
    private GameObject camCenter;

    // Offset values of headPosition to convert kamera sigth into real world position
    private const int HEAD_WIDTH_OFFSET = 350;
    private const int HEAD_HEIGTH_OFFSET = 220;
    private const int HEAD_DEPTH_OFFSET = 100000;

    private void Start()
    {
        camCenter = GameObject.Find("CamBody");
    }

    private void OnEnable()
    {
        StartCoroutine(GetFacePositionRoutine());
    }

    private void OnDisable()
    {
        StopCoroutine(GetFacePositionRoutine());
    }

    private IEnumerator GetFacePositionRoutine()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        StartCoroutine(ApiManager.Instance.RequestObjectRoutine("Face", (value) =>
        {
            this.face = JsonUtility.FromJson<Face>(value);
            StartCoroutine(GetFacePositionRoutine());
        }));
    }

    //better for continus movement
    private void FixedUpdate()
    {
        SetHeadPosition();
        SetHeadRotation();
    }

    public void SetHeadPosition()
    {
        if (this.face?.height == 0)
            return;

        float xCenter = (this.face.xPoint + (this.face.width / 2));
        float yCenter = (this.face.yPoint + (this.face.height / 2));

        // Every axies needs to be inverted except for the y axies because it is physically upside down.
        float headWidth = HEAD_WIDTH_OFFSET - xCenter;
        float headHeigth = yCenter - HEAD_HEIGTH_OFFSET;
        // To position the head in front of camera using the local cam position
        // and adding the inverted OFFSET of the face
        float headDepth = this.camCenter.transform.localPosition.z - (HEAD_DEPTH_OFFSET / this.face.height);

        this.transform.localPosition = new Vector3(headWidth, headHeigth, headDepth);
    }

    public void SetHeadRotation()
    {
        Vector3 targetDirection = this.camCenter.transform.position - this.transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        this.transform.rotation = targetRotation;
    }
}
