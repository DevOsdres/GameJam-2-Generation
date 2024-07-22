using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float mouseSensitivity = 2f;
    public float maxUpAngle = 80f;
    public float maxDownAngle = -80f;

    private float rotationX = 0f;

    void Update()
    {
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, maxDownAngle, maxUpAngle);

        transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
    }
}
