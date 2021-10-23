using UnityEngine;
using UnityEngine.UI;

public class CamController : MonoBehaviour
{
    [SerializeField] private float roataionSpeed = 5;
    [SerializeField] private GameObject cam;

    public void OnDrag()
    {
        if (Input.GetMouseButton(0))
        {
            this.transform.Rotate(new Vector3(Input.GetAxis("Mouse Y") * roataionSpeed, -Input.GetAxis("Mouse X") * roataionSpeed, 0));

            if (this.transform.rotation.eulerAngles.x > 50 && this.transform.rotation.eulerAngles.x < 100)
                this.transform.rotation = Quaternion.Euler(50, transform.rotation.eulerAngles.y, 0);
            else if (this.transform.rotation.eulerAngles.x < 300 && this.transform.rotation.eulerAngles.x > 200)
                this.transform.rotation = Quaternion.Euler(300, transform.rotation.eulerAngles.y, 0);
            else
                this.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
        }
    }

    public void OnZoomValueChange(Slider slider)
    {
        cam.transform.localPosition = new Vector3(cam.transform.localPosition.x, cam.transform.localPosition.y, slider.value);
    }
}