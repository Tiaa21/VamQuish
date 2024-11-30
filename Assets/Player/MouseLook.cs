using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;

    public float topClamp = 90f;
    public float bottomClamp = -90f;

    public Transform cameraTransform;
    public float bobbingSpeed = 14f;
    public float bobbingAmount = 0.05f;

    private float xRotation = 0f;
    private float yRotation = 0f;
    private float defaultYPos;
    private float bobbingTimer;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        defaultYPos = cameraTransform.localPosition.y;
    }

    void Update()
    {
        // Mouse Look
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, bottomClamp, topClamp);

        yRotation += mouseX;
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);

        // Head Bobbing
        HandleHeadBobbing();
    }

    void HandleHeadBobbing()
    {
        if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f || Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f)
        {
            bobbingTimer += Time.deltaTime * bobbingSpeed;
            float bobbingOffset = Mathf.Sin(bobbingTimer) * bobbingAmount;
            cameraTransform.localPosition = new Vector3(cameraTransform.localPosition.x, defaultYPos + bobbingOffset, cameraTransform.localPosition.z);
        }
    }

}

