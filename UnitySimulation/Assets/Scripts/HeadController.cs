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
        Vector3 targetDirection = camCenter.transform.position - this.gameObject.transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        this.transform.rotation = targetRotation;

        Debug.Log(this.gameObject.transform.position);
    }
}
