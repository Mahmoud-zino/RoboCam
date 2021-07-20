using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotJoint : MonoBehaviour
{
    public Vector3 Axis;
    public Vector3 StartOffset;

    public float minAngle;
    public float maxAngle;

    private void Awake()
    {
        StartOffset = transform.localPosition;
    }
}
