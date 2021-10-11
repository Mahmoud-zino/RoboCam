using UnityEngine;

public class ScreenShotManager : MonoBehaviour
{
    private int SCREEN_MID_SPAN = 100;
    private int FACE_OFFSET = 15;

    private float screenshotTimer = 3.0f;

    public bool IsFaceInScreenMiddle()
    {
        if (ApiManager.Instance.FaceCount?.faceCount != 1)
            return false;


        Vector2 screenMiddle = new Vector2(ApiManager.Instance.RaspCamera.width / 2, ApiManager.Instance.RaspCamera.height / 2);

        Vector2 faceMiddle = new Vector2(ApiManager.Instance.Face.xPoint + (ApiManager.Instance.Face.width / 2),
            ApiManager.Instance.Face.yPoint + (ApiManager.Instance.Face.height / 2));

        Vector3 destination = new Vector3(faceMiddle.x - screenMiddle.x, faceMiddle.y - screenMiddle.y, SCREEN_MID_SPAN - ApiManager.Instance.Face.width);

        // face in bound of box in all 3 directions
        if ((destination.x > -FACE_OFFSET && destination.x < FACE_OFFSET)
            && (destination.y > -FACE_OFFSET && destination.y < FACE_OFFSET)
            && (destination.z > -FACE_OFFSET && destination.z < FACE_OFFSET))
            return true;
        return false;
    }


    private void Update()
    {
        if (!IsFaceInScreenMiddle())
            screenshotTimer = 3.0f;
        else
        {
            screenshotTimer -= Time.deltaTime;
            if (screenshotTimer < 0)
            {
                //Take screen shot
            }
        }
    }
}