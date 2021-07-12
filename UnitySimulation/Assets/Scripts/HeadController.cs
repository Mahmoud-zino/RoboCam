using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HeadController : MonoBehaviour
{
    [SerializeField]
    private GameObject camGO;

    private new Camera camera;
    private Face face;
    private FaceCount faceCount;

    private void Start()
    {
        camera = new Camera();
        face = new Face();
        faceCount = new FaceCount();

    }

    private void Update()
    {
        this.transform.LookAt(camGO.transform);
        //Debug.Log($"Camera Info => height: {camera.Height}, width = {camera.Width}");
    }

}
