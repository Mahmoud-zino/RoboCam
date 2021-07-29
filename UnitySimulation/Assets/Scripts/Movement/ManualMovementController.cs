using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManualMovementController : MovementController
{
    [SerializeField]
    private Slider[] sliders;

    private void OnEnable()
    {
        MoveRobotArmOnSliderValueChange();
    }

    public void OnSendPositionClick(Button btnSelf)
    {
        int[] sliderVals = new int[] { (int)sliders[0].value, (int)sliders[1].value,
                (int)sliders[2].value, (int)sliders[3].value };

        if (!base.SendPositionCommand(sliderVals))
            return;

        btnSelf.interactable = false;
        StartCoroutine(DetectPhysicalMotorsAtPosition(btnSelf));
    }

    public void MoveRobotArmOnSliderValueChange()
    {
        int[] sliderVals = new int[] { (int)sliders[0].value, (int)sliders[1].value,
                (int)sliders[2].value, (int)sliders[3].value };
        base.MoveUnityRobotArm(sliderVals);
    }

    private IEnumerator DetectPhysicalMotorsAtPosition(Button sendBtn)
    {
        yield return new WaitForSecondsRealtime(0.5f);

        //Get position Command
        SerialConnectionManager.Instance.SendSerialMessage("G");

        string message = SerialConnectionManager.Instance.RecieveSerialMessage();

        if ((!string.IsNullOrEmpty(message)) && message.StartsWith("G"))
        {
            int[] vals = message.ExtractMotorValues();
            for (int i = 0; i < vals.Length; i++)
            {
                //physical motors are not at position yet
                if ((sliders[i].value < vals[i] - 1) || (sliders[i].value > vals[i] + 1))
                    //Relaunch the coroutine
                    yield return DetectPhysicalMotorsAtPosition(sendBtn);
                else
                {
                    SerialConnectionManager.Instance.FlushData();
                    sendBtn.interactable = true;
                }
            }
        }
        else
            yield return DetectPhysicalMotorsAtPosition(sendBtn);
    }
}
