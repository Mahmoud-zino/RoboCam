using System.Collections;
using UnityEngine;

public class HeadController : MonoBehaviour
{
    private Face face = new Face();
    private GameObject camCenter;

    // Offset values of headPosition to convert kamera sigth into real world position
    private const int HEAD_WIDTH_OFFSET = 350;
    private const int HEAD_HEIGTH_OFFSET = 220;
    private const int HEAD_DEPTH_OFFSET = 250 * 400;

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
    private void Update()
    {
        SetHeadDimesionalPosition();
        SetHeadDirectionRotation();
    }

    public void SetHeadDimesionalPosition()
    {
        if (this.face?.height == 0)
            return;
        
        // Every axies needs to be inverted except for the y axies because it is physically upside down.
        var headWidth = HEAD_WIDTH_OFFSET - (this.face.xPoint + (this.face.width / 2));
        var headHeigth = (this.face.yPoint + (this.face.height / 2)) - HEAD_HEIGTH_OFFSET;
        var headDepth = this.camCenter.transform.localPosition.z - (HEAD_DEPTH_OFFSET / this.face.height);

        this.transform.localPosition = new Vector3(headWidth, headHeigth, headDepth);
    }

    public void SetHeadDirectionRotation()
    {
        Vector3 targetDirection = camCenter.transform.position - this.gameObject.transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        this.transform.rotation = targetRotation;
    }
}
