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

        Face face = ApiManager.Instance.Face;
        Vector2 faceMiddle = new Vector2(face.xPoint + (face.width / 2), face.yPoint + (face.height / 2));

        Vector3 destination = new Vector3(faceMiddle.x - screenMiddle.x, faceMiddle.y - screenMiddle.y, SCREEN_MID_SPAN - face.width);

        int[] resultPositions = base.GetCurrentPositions();

        //Base
        resultPositions[0] = GetHorizontalPostion(resultPositions[0], (int)destination.x);

        //Wrist
        resultPositions[3] = GetVerticalPosition(resultPositions[3], (int)destination.y);

        return resultPositions;
    }

    private int GetHorizontalPostion(int currentPos, int destination)
    {
        //Target moved to the left more than the offset(-10) & its in limits 
        if(destination < -FACE_OFFSET && currentPos < base.baseLimit.y)
            return currentPos + 1;

        //Target moved to the right more than the offset(10) & its in limits 
        if (destination > FACE_OFFSET && currentPos > base.baseLimit.x)
            return currentPos - 1;

        return currentPos;
    }

    private int GetVerticalPosition(int currentPos, int destination)
    {
        if (destination > FACE_OFFSET && currentPos < base.wristLimit.y)
            return currentPos + 1;
        else if (destination < -FACE_OFFSET && currentPos < base.wristLimit.y)
            return currentPos - 1;

        return currentPos;
    }
}
