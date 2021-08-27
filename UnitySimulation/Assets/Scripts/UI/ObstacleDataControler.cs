using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class ObstacleDataControler : MonoBehaviour
{
    private GameObject cube;
    [SerializeField] private TMP_InputField locationX;
    [SerializeField] private TMP_InputField locationY;
    [SerializeField] private TMP_InputField locationZ;
    [SerializeField] private TMP_InputField RotationX;
    [SerializeField] private TMP_InputField RotationY;
    [SerializeField] private TMP_InputField RotationZ;
    [SerializeField] private TMP_InputField ScaleX;
    [SerializeField] private TMP_InputField ScaleY;
    [SerializeField] private TMP_InputField ScaleZ;

    private List<GameObject> obstaclesRef;

    private void SetObstaclesReference(List<GameObject> obstaclesRef)
    {
        this.obstaclesRef = obstaclesRef;
    }

    private void SetCubeRef(GameObject cube)
    {
        this.cube = cube;
    }

    public void DeleteThisObstacle()
    {
        obstaclesRef.Remove(this.gameObject);
        Destroy(this.gameObject);
    }

    public void OnCubeValueChanged(TMP_InputField selfField)
    {
        try
        {
            Vector3 pos = new Vector3(float.Parse(locationX.text) * 10, float.Parse(locationY.text) * 10, float.Parse(locationZ.text) * 10);
            Quaternion rot = new Quaternion(float.Parse(RotationX.text) * 10, float.Parse(RotationY.text) * 10, float.Parse(RotationZ.text) * 10, 0);
            Vector3 scale = new Vector3(float.Parse(ScaleX.text) * 10, float.Parse(ScaleY.text) * 10, float.Parse(ScaleZ.text) * 10);
            cube.transform.SetPositionAndRotation(pos, rot);
            cube.transform.localScale = scale;
        }
        catch (Exception)
        {
            return;
        }
    }
}
