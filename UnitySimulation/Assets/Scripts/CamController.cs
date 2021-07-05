using System.Collections;
using System.Collections.Generic;
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
        this.transform.Rotate(Vector3.up * roataionSpeed * horizontalInput * Time.deltaTime);

        float verticalInput = Input.GetAxis("Vertical");
        this.transform.Translate(Vector3.forward * movementSpeed * verticalInput * Time.deltaTime);
    }
}
