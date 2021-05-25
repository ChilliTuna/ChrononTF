using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    public GameObject player;

    public float sensitivity = 1;

    float xRot = 0;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        xRot -= mouseY;
        xRot = Mathf.Clamp(xRot, -90, 90);

        transform.localRotation = Quaternion.Euler(xRot, 0, 0);
        player.transform.Rotate(Vector3.up * mouseX);
    }
}
