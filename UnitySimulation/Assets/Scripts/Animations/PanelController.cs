using UnityEngine;

public abstract class PanelController : MonoBehaviour
{
    public bool IsPanelOpen { get; protected set; }

    protected Animator animator;

    public virtual void SetPanel(bool state)
    {
        animator.SetBool("Open", state);
        this.IsPanelOpen = state;
    }

    public virtual void TriggerPanel()
    {
        IsPanelOpen = !IsPanelOpen;
        animator.SetBool("Open", IsPanelOpen);
    }
}