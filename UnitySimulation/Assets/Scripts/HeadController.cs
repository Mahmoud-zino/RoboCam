using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HeadController : MonoBehaviour
{
    [SerializeField]
    private GameObject camGO;


    //better for continus movement
    private void FixedUpdate()
    {
        this.transform.LookAt(camGO.transform);
    }
}
