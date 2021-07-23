using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AutoMovementController : MovementController
{

    private const int SCREEN_MID_SPAN = 100;
    private const int FACE_OFFSET = 10;

    private void FixedUpdate()
    {
        if (ApiManager.Instance.FaceCount.faceCount != 1)
            return;

        base.MoveUnityRobotArm(CalculateTargetPos());
    }

    public int[] CalculateTargetPos()
    {
        Camera raspCam = ApiManager.Instance.RaspCamera;
        Vector2 screenMiddle = new Vector2(raspCam.width / 2, raspCam.height / 2);
        Debug.Log($"Screen midle x: {screenMiddle.x}, y: {screenMiddle.y}");

        Face face = ApiManager.Instance.Face;
        Vector2 faceMiddle = new Vector2(face.xPoint + (face.width / 2), face.yPoint + (face.height / 2));
        Debug.Log($"face midle x: {faceMiddle.x}, y: {faceMiddle.y}");

        Vector3 destination = new Vector3(faceMiddle.x - screenMiddle.x, faceMiddle.y - screenMiddle.y, SCREEN_MID_SPAN - face.width);
        Debug.Log($"destination x: {destination.x}, y: {destination.y}, z: {destination.z}");

        int[] resultPositions = base.GetCurrentPositions();

        //Base
        resultPositions[0] = GetHorizontalPostion(resultPositions[0], (int)destination.x);

        ////Wrist
        //resultPositions[3] = GetVerticalPosition(resultPositions[3], (int)destination.y);

        //resultPositions = GetZoomPosition(resultPositions, (int)destination.z);

        Debug.Log(resultPositions[0]);

        return resultPositions;
    }

    private int GetHorizontalPostion(int currentPos, int destination)
    {
        //Target moved to the left more than the offset(-10) & its in limits 
        if(destination < -FACE_OFFSET)
            if(currentPos < base.baseLimit.Max)
                return currentPos + 1;
        //Target moved to the right more than the offset(10) & its in limits 
        else if (destination > FACE_OFFSET)
               if(currentPos > base.baseLimit.Min)
                return currentPos - 1;
        return currentPos;
    }

    private int GetVerticalPosition(int currentPos, int destination)
    {
        //Y is inverted 
        //Target moved down
        if (destination > FACE_OFFSET && currentPos < base.wristLimit.Max)
            return currentPos + 1;
        //Target moved up
        else if (destination < -FACE_OFFSET && currentPos > base.wristLimit.Min)
            return currentPos - 1;

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
