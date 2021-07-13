using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HeadController : MonoBehaviour
{
    private GameObject camGO;

    private void Start()
    {
        camGO = GameObject.Find("CamBody");
    }


    //better for continus movement
    private void FixedUpdate()
    {
        this.transform.LookAt(camGO.transform);
    }
}
