using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    public Camera theCamera;
    public Vector2 cameraVelocity;
    public Vector2 previousMoveInput;
    public Vector2 thisFrameMoveInput;
    public float movespeed;
    public float velocitySmoothingWithInput = 0.00001f;
    public float velocitySmoothingNoInput = 0.00001f;
    public Vector2 normalizedMousePositionOnScreen;
    private void Awake()
    {
        instance = this;
        theCamera = GetComponent<Camera>();
        Cursor.lockState = CursorLockMode.Confined;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.lockState == CursorLockMode.Confined)
                Cursor.lockState = CursorLockMode.None;
            else
                Cursor.lockState = CursorLockMode.Confined;
        }

        previousMoveInput = thisFrameMoveInput;
        thisFrameMoveInput.x = Input.GetAxisRaw("Horizontal");
        thisFrameMoveInput.y = Input.GetAxisRaw("Vertical");
        //thisFrameMoveInput.Normalize();

        if (thisFrameMoveInput.magnitude >= 0.01f)
            SetCameraVelocity();
        else
        {
            normalizedMousePositionOnScreen = Input.mousePosition;
            normalizedMousePositionOnScreen.x /= Screen.width;
            normalizedMousePositionOnScreen.y /= Screen.height;

            if (normalizedMousePositionOnScreen.x <= 0.01f)
                thisFrameMoveInput.x = -1f;
            else if (normalizedMousePositionOnScreen.x >= 0.99f)
                thisFrameMoveInput.x = 1f;

            if (normalizedMousePositionOnScreen.y <= 0.01f)
                thisFrameMoveInput.y = -1f;
            else if (normalizedMousePositionOnScreen.y >= 0.99f)
                thisFrameMoveInput.y = 1f;

            //thisFrameMoveInput.Normalize();

            SetCameraVelocity();
        }
        
        theCamera.transform.Translate(cameraVelocity * Time.unscaledDeltaTime * movespeed, Space.Self);

        
        //DebugTextCanvas.SetDbText("mouse pos", "mousePos: " + normalizedMousePositionOnScreen.ToString("0.0000"));
    }
    void SetCameraVelocity()
    {
        if (thisFrameMoveInput.magnitude >= 0.01f)
            cameraVelocity = SmoothFunc.Damp(cameraVelocity, thisFrameMoveInput, velocitySmoothingWithInput, Time.unscaledDeltaTime);
        else
            cameraVelocity = SmoothFunc.Damp(cameraVelocity, thisFrameMoveInput, velocitySmoothingNoInput, Time.unscaledDeltaTime);
    }
}
