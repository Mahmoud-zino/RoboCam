using UnityEngine;

public class SerialAnimManager : MonoBehaviour
{
    private Animator anim;

    private void Start()
    {
        anim = this.GetComponent<Animator>();
    }

    public void TriggerSerialSettingsMenu()
    {
        anim.SetBool("IsOpened", !anim.GetBool("IsOpened"));
    }
}
