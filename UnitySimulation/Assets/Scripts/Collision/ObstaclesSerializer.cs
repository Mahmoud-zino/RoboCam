using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ObstaclesSerializer : MonoBehaviour
{
    [SerializeField] private GameObject obstacle;
    private string saveFilePath;

    private void Awake()
    {
        saveFilePath = $"{Application.persistentDataPath}/obstacleData.json";
    }

    public List<Cube> LoadCubes()
    {
        if (!File.Exists(saveFilePath))
            return new List<Cube>();

        string json = File.ReadAllText(saveFilePath);
        return JsonUtility.FromJson<CubeSerializer>(json).Cubes;
    }

    public void SaveCubes(List<Cube> cubes)
    {
        CubeSerializer cubeSerializer = new CubeSerializer(cubes);
        string json = JsonUtility.ToJson(cubeSerializer);
        File.WriteAllText(saveFilePath, json);
    }
}
