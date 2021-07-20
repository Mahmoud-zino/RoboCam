using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKManager : MonoBehaviour
{
    public RobotJoint[] joints;
    public GameObject target;

    public Vector3 ForwardKinematics(float[] angles)
    {
        Vector3 prevPoint = joints[0].transform.position;
        Quaternion rotation = Quaternion.identity;

        for (int i = 1; i < joints.Length; i++)
        {
            //Rotates around a new Axis
            rotation *= Quaternion.AngleAxis(angles[i - 1], joints[i - 1].Axis);

            Vector3 nextPoint = prevPoint + rotation * joints[i].StartOffset;

            prevPoint = nextPoint;
        }
        return prevPoint;
    }

    public float DistanceFromTarget(Vector3 target, float[] angles)
    {
        Vector3 poinjt = ForwardKinematics(angles);
        return Vector3.Distance(poinjt, target);
    }

    public float PartialGradient(Vector3 target, float[] angles, int i)
    {
        //Saves the angle,
        //it will be restored later

        float angle = angles[i];

        //Gradient : [F(x+samplingDistance)-F(x)] / h
        float f_X = DistanceFromTarget(target, angles);

        angles[i] += 1;
        float f_X_plus_d = DistanceFromTarget(target, angles);

        float gradient = (f_X_plus_d - f_X);

        //restores
        angles[i] = angle;
        return gradient;
    }

    public float[] InverseKinematics(Vector3 target, float[] angles)
    {
        if (DistanceFromTarget(target, angles) < 50)
            return angles;

        for (int i = joints.Length - 1; i >= 0; i--)
        {
            //Gradient descent
            float gradient = PartialGradient(target, angles, i);
            angles[i] -= gradient;

            //clamp
            angles[i] = Mathf.Clamp(angles[i], joints[i].minAngle, joints[i].maxAngle);

            if (DistanceFromTarget(target, angles) < 50)
                return angles;
        }
        return angles;
    }

    private float[] GetAnglesFromJoints()
    {
        return new float[] { joints[0].transform.localEulerAngles.y, joints[1].transform.localEulerAngles.x, joints[2].transform.localEulerAngles.x, joints[3].transform.localEulerAngles.x };
    }

    private void SetJointsFromAngles(float[] angles)
    {
        joints[0].transform.localEulerAngles = new Vector3(joints[0].transform.localEulerAngles.x, angles[0], joints[0].transform.localEulerAngles.z);
        joints[1].transform.localEulerAngles = new Vector3(angles[1], joints[1].transform.localEulerAngles.y, joints[1].transform.localEulerAngles.z);
        joints[2].transform.localEulerAngles = new Vector3(angles[2], joints[2].transform.localEulerAngles.y, joints[2].transform.localEulerAngles.z);
        joints[3].transform.localEulerAngles = new Vector3(angles[3], joints[3].transform.localEulerAngles.y, joints[3].transform.localEulerAngles.z);
    }

    private void Update()
    {
        SetJointsFromAngles(InverseKinematics(target.transform.position, GetAnglesFromJoints()));
    }
}
