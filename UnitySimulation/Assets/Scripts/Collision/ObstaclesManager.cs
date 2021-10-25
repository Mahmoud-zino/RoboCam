using System.Collections.Generic;
using UnityEngine;

public class ObstaclesManager : MonoBehaviour
{
    private const float MILLI_TO_CENTI_CONVERSION_RATE = 10.0f;

    [SerializeField] private GameObject obstaclePanelPrefab;
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private GameObject obstaclesPanel;

    private ObstaclesSerializer obstaclesSerializer;


    private void Start()
    {
        obstaclesSerializer = this.GetComponent<ObstaclesSerializer>();
        LoadObstacles();
    }

    private PanelObstacleBehaiviour SetupObstacle()
    {
        GameObject obstaclePanel = Instantiate(obstaclePanelPrefab, obstaclesPanel.transform);

        GameObject obstacle = Instantiate(obstaclePrefab, this.transform);

        PanelObstacleBehaiviour panelObstacleBehaiviour = obstaclePanel.GetComponent<PanelObstacleBehaiviour>();

        panelObstacleBehaiviour.SetPhysicalObstacleRef(obstacle);
        panelObstacleBehaiviour.SetManager(this);


        return panelObstacleBehaiviour;
    }

    //called from gui
    public void AddObstacle()
    {
        this.SetupObstacle();

        SaveObstacles();
    }

    public void AddObstacle(Cube cube)
    {
        PanelObstacleBehaiviour panelObstacleBehaiviour = this.SetupObstacle();

        if (cube != null)
            panelObstacleBehaiviour.SetStartupData(cube);
    }

    public void LoadObstacles()
    {
        List<Cube> cubes = obstaclesSerializer.LoadCubes();

        foreach (Cube cube in cubes)
            AddObstacle(cube);
    }

    public void SaveObstacles()
    {
        List<Cube> cubes = new List<Cube>();
        foreach (Transform t in this.transform)
        {
            cubes.Add(new Cube()
            {
                Position = t.position / MILLI_TO_CENTI_CONVERSION_RATE,
                Rotation = t.eulerAngles / MILLI_TO_CENTI_CONVERSION_RATE,
                Scale = t.localScale / MILLI_TO_CENTI_CONVERSION_RATE
            });
        }

        obstaclesSerializer.SaveCubes(cubes);
    }

    private void OnApplicationQuit()
    {
        this.SaveObstacles();
    }
}
