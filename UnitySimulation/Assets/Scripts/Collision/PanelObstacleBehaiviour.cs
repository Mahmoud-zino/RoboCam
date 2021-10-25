using TMPro;
using UnityEngine;

public class PanelObstacleBehaiviour : MonoBehaviour
{
    private const float MILLI_TO_CENTI_CONVERSION_RATE = 10.0f;

    [SerializeField] private TMP_InputField locationX;
    [SerializeField] private TMP_InputField locationY;
    [SerializeField] private TMP_InputField locationZ;
    [SerializeField] private TMP_InputField RotationX;
    [SerializeField] private TMP_InputField RotationY;
    [SerializeField] private TMP_InputField RotationZ;
    [SerializeField] private TMP_InputField ScaleX;
    [SerializeField] private TMP_InputField ScaleY;
    [SerializeField] private TMP_InputField ScaleZ;


    private GameObject physicalObstacle;
    private ObstaclesManager obstaclesManager;



    public void SetPhysicalObstacleRef(GameObject physicalObstacle)
    {
        this.physicalObstacle = physicalObstacle;
    }

    public void SetStartupData(Cube cube)
    {
        locationX.text = cube.Position.x.ToString();
        locationY.text = cube.Position.y.ToString();
        locationZ.text = cube.Position.z.ToString();
        RotationX.text = cube.Rotation.x.ToString();
        RotationY.text = cube.Rotation.y.ToString();
        RotationZ.text = cube.Rotation.z.ToString();
        ScaleX.text = cube.Scale.x.ToString();
        ScaleY.text = cube.Scale.y.ToString();
        ScaleZ.text = cube.Scale.z.ToString();
    }

    public void SetManager(ObstaclesManager obstaclesManager)
    {
        this.obstaclesManager = obstaclesManager;
    }


    public void DeleteObstacle()
    {
        Destroy(this.gameObject);
        Destroy(this.physicalObstacle);
        this.obstaclesManager.SaveObstacles();
    }

    public void OnCubeValueChanged()
    {
        locationX.text = locationX.text.Replace(".", ",");
        locationY.text = locationY.text.Replace(".", ",");
        locationZ.text = locationZ.text.Replace(".", ",");
        RotationX.text = RotationX.text.Replace(".", ",");
        RotationY.text = RotationY.text.Replace(".", ",");
        RotationZ.text = RotationZ.text.Replace(".", ",");
        ScaleX.text = ScaleX.text.Replace(".", ",");
        ScaleY.text = ScaleY.text.Replace(".", ",");
        ScaleZ.text = ScaleZ.text.Replace(".", ",");

        try
        {
            Cube cube = new Cube
            {
                Position = new Vector3(float.Parse(locationX.text) * MILLI_TO_CENTI_CONVERSION_RATE, float.Parse(locationY.text) * MILLI_TO_CENTI_CONVERSION_RATE, float.Parse(locationZ.text) * MILLI_TO_CENTI_CONVERSION_RATE),
                Rotation = new Vector3(float.Parse(RotationX.text) * MILLI_TO_CENTI_CONVERSION_RATE, float.Parse(RotationY.text) * MILLI_TO_CENTI_CONVERSION_RATE, float.Parse(RotationZ.text) * MILLI_TO_CENTI_CONVERSION_RATE),
                Scale = new Vector3(float.Parse(ScaleX.text) * MILLI_TO_CENTI_CONVERSION_RATE, float.Parse(ScaleY.text) * MILLI_TO_CENTI_CONVERSION_RATE, float.Parse(ScaleZ.text) * MILLI_TO_CENTI_CONVERSION_RATE)
            };

            ChangePhyiscalObstacleData(cube);
            this.obstaclesManager.SaveObstacles();
        }
        catch
        {
        }
    }

    private void ChangePhyiscalObstacleData(Cube cube)
    {
        physicalObstacle.transform.position = cube.Position;
        physicalObstacle.transform.eulerAngles = WrapRotationAngles(cube.Rotation);
        physicalObstacle.transform.localScale = cube.Scale;
    }

    private Vector3 WrapRotationAngles(Vector3 angles)
    {
        Vector3 result = new Vector3();

        result.x = WrapAngle(angles.x);
        result.y = WrapAngle(angles.y);
        result.z = WrapAngle(angles.z);

        return result;
    }

    private float WrapAngle(float angle)
    {
        angle %= 360;
        if (angle > 180)
            return angle - 360;
        return angle;
    }
}
