using UnityEngine;

public class CamController : MonoBehaviour
{
    [SerializeField]
    private float roataionSpeed = 50;
    [SerializeField]
    private float movementSpeed = 100;

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        this.transform.Rotate(horizontalInput * roataionSpeed * Time.deltaTime * Vector3.up);

        float verticalInput = Input.GetAxis("Vertical");
        this.transform.Translate(verticalInput * movementSpeed * Time.deltaTime * Vector3.forward);
    }
}
