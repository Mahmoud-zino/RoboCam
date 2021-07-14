using System.Collections;
using UnityEngine;

public class HeadController : MonoBehaviour
{
    [SerializeField]
    private GameObject camGO;

    private Camera raspCamera = new Camera();
    private Face face = new Face();

    private void Start()
    {
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
    private void FixedUpdate()
    {
        this.transform.LookAt(camGO.transform);

        SetHeadPosition();
    }

    private void SetHeadPosition()
    {
        this.transform.position = new Vector3(this.camGO.transform.position.x, this.camGO.transform.position.y, this.camGO.transform.position.z + 20);
    }
}
