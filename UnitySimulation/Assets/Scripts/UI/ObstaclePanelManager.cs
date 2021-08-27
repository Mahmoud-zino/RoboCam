using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObstaclePanelManager : MonoBehaviour
{
    [SerializeField]
    private GameObject Obstacle;
    [SerializeField]
    private GameObject cube;
    [SerializeField]
    private GameObject ObstaclesPanel;
    private List<GameObject> obstacles = new List<GameObject>();

    private bool isPanelOpen = false;
    private Animator anim;

    private void Start()
    {
        anim = this.GetComponent<Animator>();
    }

    public void TriggerPanel()
    {
        isPanelOpen = !isPanelOpen;
        anim.SetBool("IsPanelOpen", isPanelOpen);
    }

    public void AddObstacle()
    {
        GameObject ob = Instantiate(Obstacle, ObstaclesPanel.transform);
        obstacles.Add(ob);
        ob.SendMessage("SetObstaclesReference", obstacles);
        GameObject cubeInstance = Instantiate(cube, GameObject.Find("CustomObstacles").transform);
        ob.SendMessage("SetCubeRef", cubeInstance);
    }
}
