using UnityEngine;

public class PanelAnimManager : MonoBehaviour
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
        SetPanel(isPanelOpen);
    }

    public void SetPanel(bool state)
    {
        Debug.Log(anim.name);
        isPanelOpen = state;
        anim.SetBool("IsPanelOpen", isPanelOpen);
    }
}
