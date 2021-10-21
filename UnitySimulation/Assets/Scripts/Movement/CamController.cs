using UnityEngine;

public class CamController : MonoBehaviour
{
    [SerializeField] private float roataionSpeed = 100;

    private void LateUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        this.transform.eulerAngles += new Vector3(verticalInput * roataionSpeed * Time.deltaTime, horizontalInput * roataionSpeed * Time.deltaTime, 0);
    }
}
