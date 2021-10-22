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

    public void OnSendPositionClick()
    {
        if(this.IsRobotColliding())
            return;

        int[] sliderVals = new int[] { (int)sliders[0].value, (int)sliders[1].value,
                (int)sliders[2].value, (int)sliders[3].value };

        base.SendPositionCommand(sliderVals);
    }

    public void MoveRobotArmOnSliderValueChange()
    {
        int[] sliderVals = new int[] { (int)sliders[0].value, (int)sliders[1].value,
                (int)sliders[2].value, (int)sliders[3].value };
        base.MoveUnityRobotArm(sliderVals);
    }
}
