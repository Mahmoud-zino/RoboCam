using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObstaclesManager : MonoBehaviour
{
    [SerializeField] private GameObject obstaclePanelPrefab;
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private GameObject obstaclesPanel;
    private ObstaclesSerializer obstaclesSerializer;

    private void Start()
    {
        obstaclesSerializer = this.GetComponent<ObstaclesSerializer>();
        LoadObstacles();
    }

    //called from gui
    public void AddObstacle()
    {
        GameObject obstaclePanel = Instantiate(obstaclePanelPrefab, obstaclesPanel.transform);

        GameObject obstacle = Instantiate(obstaclePrefab, this.transform);

        obstaclePanel.SendMessage("SetPhysicalObstacleRef", obstacle);

        obstaclePanel.SendMessage("SetManager", this);

        SaveObstacles();
    }

    public void AddObstacle(Cube cube)
    {
        GameObject obstaclePanel = Instantiate(obstaclePanelPrefab, obstaclesPanel.transform);

        GameObject obstacle = Instantiate(obstaclePrefab, this.transform);

        obstaclePanel.SendMessage("SetPhysicalObstacleRef", obstacle);

        obstaclePanel.SendMessage("SetManager", this);

        if (cube != null)
        {
            obstaclePanel.SendMessage("SetStartupData", cube);
        }
    }

    public void LoadObstacles()
    {
        List<Cube> cubes = obstaclesSerializer.LoadCubes();

        foreach (Cube cube in cubes)
        {
            AddObstacle(cube);
        }
    }

    public void SaveObstacles()
    {
        List<Cube> cubes = new List<Cube>();
        foreach (Transform t in this.transform)
        {
            cubes.Add(new Cube()
            {
                Position = t.position / 10,
                Rotation = t.eulerAngles / 10,
                Scale = t.localScale / 10
            });
        }

        obstaclesSerializer.SaveCubes(cubes);
    }

    private void OnApplicationQuit()
    {
        this.SaveObstacles();
    }
}
