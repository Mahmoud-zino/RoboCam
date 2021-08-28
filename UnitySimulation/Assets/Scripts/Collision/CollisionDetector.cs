using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    public bool IsColliding { get; private set; }
    public Collider ColliderObj { get; private set; }
    private void OnTriggerEnter(Collider other)
    {
        IsColliding = true;
        ColliderObj = other;
    }

    private void OnTriggerExit(Collider other)
    {
        IsColliding = false;
        ColliderObj = null;
    }
}
