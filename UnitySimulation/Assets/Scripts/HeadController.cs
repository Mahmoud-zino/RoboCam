using System.Collections;
using UnityEngine;

public class HeadController : MonoBehaviour
{
    private Camera raspCamera = new Camera();
    private Face face = new Face();

    private GameObject camCenter;

    private void Start()
    {
        camCenter = GameObject.Find("CamBody");

        StartCoroutine(ApiManager.Instance.RequestObjectRoutine("Camera", (value) =>
        {
            this.raspCamera = JsonUtility.FromJson<Camera>(value);
        }));
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
        if (this.face.height == 0)
            return;
        this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, this.camCenter.transform.localPosition.z - (250 * 400 / this.face.height));
    }

    public void SetHeadDirectionRotation()
    {
        Vector3 targetDirection = camCenter.transform.position - this.gameObject.transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        this.transform.rotation = targetRotation;
    }
}
