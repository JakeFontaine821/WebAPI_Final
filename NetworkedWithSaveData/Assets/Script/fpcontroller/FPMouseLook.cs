using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPMouseLook : MonoBehaviour
{
    public float mSensitivity = 100.0f;
    public Transform playerObj;
    float xRot = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //Gets input from mouse or joystick
        float mouseX = Input.GetAxis("Mouse X") * mSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mSensitivity * Time.deltaTime;

        //rotate Camera up and down
        xRot -= mouseY;
        xRot = Mathf.Clamp(xRot, -90, 90);

        transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);

        playerObj.Rotate(Vector3.up * mouseX);
    }
}
