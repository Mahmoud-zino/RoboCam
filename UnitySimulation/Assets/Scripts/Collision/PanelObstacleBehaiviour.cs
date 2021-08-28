using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using System.Globalization;

public class PanelObstacleBehaiviour : MonoBehaviour
{
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

    private void SetPhysicalObstacleRef(GameObject physicalObstacle)
    {
        this.physicalObstacle = physicalObstacle;
    }

    private void SetStartupData(Cube cube)
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

    private void SetManager(ObstaclesManager obstaclesManager)
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
        try
        {
            Cube cube = new Cube
            {
                Position = new Vector3(float.Parse(locationX.text, CultureInfo.InvariantCulture) * 10, float.Parse(locationY.text, CultureInfo.InvariantCulture) * 10, float.Parse(locationZ.text, CultureInfo.InvariantCulture) * 10),
                Rotation = new Vector3(float.Parse(RotationX.text, CultureInfo.InvariantCulture) * 10, float.Parse(RotationY.text, CultureInfo.InvariantCulture) * 10, float.Parse(RotationZ.text, CultureInfo.InvariantCulture) * 10),
                Scale = new Vector3(float.Parse(ScaleX.text, CultureInfo.InvariantCulture) * 10, float.Parse(ScaleY.text, CultureInfo.InvariantCulture) * 10, float.Parse(ScaleZ.text, CultureInfo.InvariantCulture) * 10)
            };

            ChangePhyiscalObstacleData(cube);
            this.obstaclesManager.SaveObstacles();
        }
        catch (Exception)
        {
            return;
        }
    }

    private void ChangePhyiscalObstacleData(Cube cube)
    {
        physicalObstacle.transform.position = cube.Position;
        physicalObstacle.transform.eulerAngles = cube.Rotation;
        physicalObstacle.transform.localScale = cube.Scale;
    }
}
