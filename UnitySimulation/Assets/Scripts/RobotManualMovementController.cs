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

    private void Start()
    {
        MoveRobotOnSliderValueChange();
    }

    public void OnSendPositionClick(Button btnSelf)
    {
        if (!SerialConnectionManager.Instance.IsConnected())
            return;

        int[] sliderVals = new int[] { (int)sliders[0].value, (int)sliders[1].value,
                (int)sliders[2].value, (int)sliders[3].value };

        string movementCommand = sliderVals.BuildMovementCommand();

        SerialConnectionManager.Instance.SendSerialMessage(movementCommand);
        Debug.Log($"Move Command: {movementCommand}");

        btnSelf.interactable = false;

        StartCoroutine(DetectPhysicalMotorsAtPosition(btnSelf));
    }

    public void MoveRobotOnSliderValueChange()
    {
        //Parent object (Base MOtor) can be controled using eulerAngles
        motors[0].transform.eulerAngles = new Vector3(motors[0].transform.eulerAngles.x, sliders[0].value,
            motors[0].transform.eulerAngles.z);

        //Because the Rotation in unity is not the same in real life the value should be shifted by -90 and inverted
        motors[1].transform.localRotation = Quaternion.Euler(-(sliders[1].value + valueShift + shoulderShift),
            motors[1].transform.localRotation.y, motors[1].transform.localRotation.z);

        motors[2].transform.localRotation = Quaternion.Euler(-(sliders[2].value + valueShift + elbowShift),
            motors[2].transform.localRotation.y, motors[2].transform.localRotation.z);

        motors[3].transform.localRotation = Quaternion.Euler(-(sliders[3].value + valueShift),
            motors[3].transform.localRotation.y, motors[3].transform.localRotation.z);
    }

    private IEnumerator DetectPhysicalMotorsAtPosition(Button sendBtn)
    {
        yield return new WaitForSecondsRealtime(0.5f);

        //Get position Command
        SerialConnectionManager.Instance.SendSerialMessage("G");

        string message = SerialConnectionManager.Instance.RecieveSerialMessage();
        Debug.Log($"Recieved Serial Message: {message}");

        if (!string.IsNullOrEmpty(message) && message.StartsWith("["))
        {
            int[] vals = message.ExtractMotorValues();
            for (int i = 0; i < vals.Length; i++)
            {
                //physical motors are not at position yet
                if (sliders[i].value < vals[i] - 1 || sliders[i].value > vals[i] + 1)
                    //Relaunch the coroutine
                    StartCoroutine(DetectPhysicalMotorsAtPosition(sendBtn));
                else
                    sendBtn.interactable = true;
            }
        }
    }

}
