using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomPanelController : MonoBehaviour
{
    private Animator anim;

    private void Start()
    {
        this.anim = this.GetComponent<Animator>();
    }

    public void OnPointerEnter()
    {
        this.anim.SetBool("Open", true);
    }

    public void OnPointerExit()
    {
        this.anim.SetBool("Open", false);
    }
}