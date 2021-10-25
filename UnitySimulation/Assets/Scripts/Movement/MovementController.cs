using UnityEngine;

//Parent class of Robot movement
public abstract class MovementController : MonoBehaviour
{
    public struct Limit
    {
        public Limit(int min, int max)
        {
            this.Min = min;
            this.Max = max;
        }
        public int Min { get; set; }
        public int Max { get; set; }
    }

    [SerializeField]
    private GameObject[] motors;

    private const int VALUE_SHIFT = -90;
    private const int SHOULDER_SHIFT = -5;
    private const int ELBOW_SHIFT = 15;

    protected Limit baseLimit = new Limit(0, 180);
    protected Limit shoulderLimit = new Limit(45, 160);
    protected Limit elbowLimit = new Limit(45, 160);
    protected Limit wristLimit = new Limit(45, 180);

    public virtual bool SendPositionCommand(int[] vals)
    {
        if (!SerialConnectionManager.Instance.IsConnected())
            return false;

        string movementCommand = vals.BuildMovementCommand();

        SerialConnectionManager.Instance.SendSerialMessage(movementCommand);
        return true;
    }

    public virtual void MoveUnityRobotArm(int[] vals)
    {
        //Parent object (Base Motor) can be controled using eulerAngles
        motors[0].transform.eulerAngles = new Vector3(motors[0].transform.eulerAngles.x, vals[0],
            motors[0].transform.eulerAngles.z);

        //Because of the child inherting its parent rotation in unity the value should be shifted by -90 and inverted
        motors[1].transform.localRotation = Quaternion.Euler(-(vals[1] + VALUE_SHIFT + SHOULDER_SHIFT),
            motors[1].transform.localRotation.y, motors[1].transform.localRotation.z);

        motors[2].transform.localRotation = Quaternion.Euler(-(vals[2] + VALUE_SHIFT + ELBOW_SHIFT),
            motors[2].transform.localRotation.y, motors[2].transform.localRotation.z);

        motors[3].transform.localRotation = Quaternion.Euler(-(vals[3] + VALUE_SHIFT),
            motors[3].transform.localRotation.y, motors[3].transform.localRotation.z);
    }

    public int[] GetCurrentPositions()
    {
        return new int[] { Mathf.RoundToInt(motors[0].transform.eulerAngles.y),
            (180 - Mathf.RoundToInt(WrapAngle(motors[1].transform.localEulerAngles.x)) + VALUE_SHIFT - SHOULDER_SHIFT),
            (180 - Mathf.RoundToInt(WrapAngle(motors[2].transform.localEulerAngles.x)) + VALUE_SHIFT - ELBOW_SHIFT),
            (180 - Mathf.RoundToInt(WrapAngle(motors[3].transform.localEulerAngles.x)) + VALUE_SHIFT)};
    }

    private float WrapAngle(float angle)
    {
        angle %= 360;
        if (angle > 180)
            return angle - 360;

        return angle;
    }

    protected bool IsRobotColliding()
    {
        foreach (CollisionDetector cd in this.GetComponentsInChildren<CollisionDetector>())
        {
            if (cd.IsColliding)
            {
                Logger.Log.Information($"Collision was detected!");
                return true;
            }
        }
        return false;
    }
}
