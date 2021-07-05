using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RobotManualMovementController : MonoBehaviour
{
    [SerializeField]
    private GameObject[] motors;
    [SerializeField]
    private Slider[] sliders;
    private readonly int valueShift = -90;
    private readonly int shoulderShift = -5;
    private readonly int elbowShift = 15;

    private void Update()
    {
        //Parent object (Base MOtor) can be controled using eulerAngles
        motors[0].transform.eulerAngles = new Vector3(motors[0].transform.eulerAngles.x, sliders[0].value, motors[0].transform.eulerAngles.z);
        //Because the Rotation in unity is not the same in real life the value should be shifted be -90 and inverted
        motors[1].transform.localRotation = Quaternion.Euler(-(sliders[1].value + valueShift + shoulderShift), motors[1].transform.localRotation.y, motors[1].transform.localRotation.z);
        motors[2].transform.localRotation = Quaternion.Euler(-(sliders[2].value + valueShift + elbowShift), motors[2].transform.localRotation.y, motors[2].transform.localRotation.z);
        motors[3].transform.localRotation = Quaternion.Euler(-(sliders[3].value + valueShift), motors[3].transform.localRotation.y, motors[3].transform.localRotation.z);
    }
}
