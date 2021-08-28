using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AutoMovementController : MovementController
{
    [SerializeField]
    private int SCREEN_MID_SPAN = 150;

    [SerializeField]
    private int FACE_OFFSET = 15;


    private void OnEnable()
    {
        StartCoroutine(MoveRobotRoutine());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator MoveRobotRoutine()
    {
        yield return new WaitForSecondsRealtime(0.2f);
        if (ApiManager.Instance.FaceCount != null && ApiManager.Instance.FaceCount.faceCount == 1)
        {
            int[] lastVals = GetCurrentPositions();

            int[] vals = CalculateTargetPos();
            base.MoveUnityRobotArm(vals);
            if(IsRobotColliding())
                base.MoveUnityRobotArm(lastVals);
            else
                base.SendPositionCommand(vals);
        }

        yield return MoveRobotRoutine();
    }

    public int[] CalculateTargetPos()
    {
        int[] targetPosition = base.GetCurrentPositions();

        Camera raspCam = ApiManager.Instance.RaspCamera;

        if (raspCam is null)
            return targetPosition;

        Vector2 screenMiddle = new Vector2(raspCam.width / 2, raspCam.height / 2);

        Face face = ApiManager.Instance.Face;
        if (face is null)
            return targetPosition;


        Vector2 faceMiddle = new Vector2(face.xPoint + (face.width / 2), face.yPoint + (face.height / 2));

        Vector3 destination = new Vector3(faceMiddle.x - screenMiddle.x, faceMiddle.y - screenMiddle.y, SCREEN_MID_SPAN - face.width);

        //Base / Horizontal
        targetPosition[0] = GetHorizontalPostion(targetPosition[0], (int)destination.x);
        //Wrist / Vertical
        targetPosition[3] = GetVerticalPosition(targetPosition[3], (int)destination.y);

        targetPosition = GetZoomPosition(targetPosition, (int)destination.z);

        return targetPosition;
    }

    private int GetHorizontalPostion(int currentPos, int destination)
    {
        //Target moved to the left more than the offset & its in limits 
        if(destination < -FACE_OFFSET)
        {
            if (currentPos < base.baseLimit.Max)
            {
                return ++currentPos;
            }
        }
        //Target moved to the right more than the offset & its in limits 
        else if (destination > FACE_OFFSET)
        {
            if (currentPos > base.baseLimit.Min)
            {
                return --currentPos;
            }
        }

        return currentPos;
    }

    private int GetVerticalPosition(int currentPos, int destination)
    {
        //Y is inverted 
        //Target moved down
        if (destination > FACE_OFFSET)
        {
            if(currentPos < base.wristLimit.Max)
                return ++currentPos;
        }
        //Target moved up
        else if (destination < -FACE_OFFSET)
        {
            if(currentPos > base.wristLimit.Min)
                return --currentPos;
        }

        return currentPos;
    }

    private int[] GetZoomPosition(int[] currentPos, int destination)
    {
        int shoulderVal = currentPos[1];
        int elbowVal = currentPos[2];
        int wristVal = currentPos[3];

        //target too close
        if(destination > FACE_OFFSET)
        {
            if(shoulderVal < base.shoulderLimit.Max)
            {
                shoulderVal++;

                if (elbowVal > base.elbowLimit.Min)
                    elbowVal--;
                else if (wristVal > base.wristLimit.Min)
                    wristVal--;
            }
            else if(elbowVal < base.elbowLimit.Max && wristVal > base.wristLimit.Min)
            {
                elbowVal++;
                wristVal--;
            }
        }
        //TArget too far away
        else if(destination < -FACE_OFFSET)
        {
            if(shoulderVal > base.shoulderLimit.Min)
            {
                shoulderVal--;

                if (elbowVal < base.elbowLimit.Max)
                    elbowVal++;
                else if (wristVal < base.wristLimit.Max)
                    wristVal++;
            }
            else if(elbowVal > base.elbowLimit.Min && wristVal < base.wristLimit.Max)
            {
                elbowVal--;
                wristVal++;
            }
        }

        currentPos[1] = shoulderVal;
        currentPos[2] = elbowVal;
        currentPos[3] = wristVal;

        return currentPos;
    }
}
