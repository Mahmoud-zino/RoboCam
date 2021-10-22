using UnityEngine;

public class HidenPanelController : PanelController
{
    private void Start()
    {
        base.animator = this.GetComponent<Animator>();
    }

    public void OnPointerEnter()
    {
        base.animator.SetBool("Open", true);
    }

    public void OnPointerExit()
    {
        base.animator.SetBool("Open", false);
    }
}