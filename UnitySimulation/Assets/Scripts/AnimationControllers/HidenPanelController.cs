using UnityEngine;

public class HidenPanelController : PanelController
{
    public void OnPointerEnter()
    {
        this.animator.SetBool("Open", true);
    }

    public void OnPointerExit()
    {
        base.animator.SetBool("Open", false);
    }
}