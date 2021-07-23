using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Parent class of Robot movement
public abstract class MovementController : MonoBehaviour
{
    [SerializeField]
    private GameObject[] motors;
    private int[] lastMotorVals = new int[4];

    private const int VALUE_SHIFT = -90;
    private const int SHOULDER_SHIFT = -5;
    private const int ELBOW_SHIFT = 15;

    //x => min, y => max
    protected Vector2 baseLimit = new Vector2(0, 180);
    protected Vector2 wristLimit = new Vector2(45, 180);

    public virtual bool SendPositionCommand(int[] vals)
    {
        if (!SerialConnectionManager.Instance.IsConnected())
            return false;

        string movementCommand = vals.BuildMovementCommand();

        SerialConnectionManager.Instance.SendSerialMessage(movementCommand);
        Debug.Log($"Move Command: {movementCommand}");
        return true;
    }

    public virtual void MoveUnityRobotArm(int[] vals)
    {
        this.lastMotorVals = vals;

        //Parent object (Base Motor) can be controled using eulerAngles
        motors[0].transform.eulerAngles = new Vector3(motors[0].transform.eulerAngles.x, vals[0],
            motors[0].transform.eulerAngles.z);

        //Because the Rotation in unity is not the same in real life the value should be shifted by -90 and inverted
        motors[1].transform.localRotation = Quaternion.Euler(-(vals[1] + VALUE_SHIFT + SHOULDER_SHIFT),
            motors[1].transform.localRotation.y, motors[1].transform.localRotation.z);

        motors[2].transform.localRotation = Quaternion.Euler(-(vals[2] + VALUE_SHIFT + ELBOW_SHIFT),
            motors[2].transform.localRotation.y, motors[2].transform.localRotation.z);

        motors[3].transform.localRotation = Quaternion.Euler(-(vals[3] + VALUE_SHIFT),
            motors[3].transform.localRotation.y, motors[3].transform.localRotation.z);
    }

    public int[] GetCurrentPositions()
    {
        return this.lastMotorVals;
    }
}
