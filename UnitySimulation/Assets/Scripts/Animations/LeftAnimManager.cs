using UnityEngine;

public class LeftAnimManager : MonoBehaviour
{
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
}
