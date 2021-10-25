using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    public bool IsColliding { get; private set; }
    private void OnTriggerEnter(Collider other)
    {
        IsColliding = true;
    }

    private void OnTriggerExit(Collider other)
    {
        IsColliding = false;
    }
}
