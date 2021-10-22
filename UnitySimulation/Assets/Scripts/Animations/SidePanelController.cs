using UnityEngine;

public class SidePanelController : PanelController
{
    private void Start()
    {
        base.animator = this.GetComponent<Animator>();
    }
}
