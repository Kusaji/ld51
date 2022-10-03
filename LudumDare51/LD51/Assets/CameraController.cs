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
    public float shakeTime;
    public float shakeDistanceMod = 1f;
    public Vector3 shakeDir;
    public float shakeSpeed;
    Vector3 cameraShakeAdd;
    public float shakeDurationMod;
    public float minCameraSize = 8f;
    public float maxCameraSize = 20f;
    public float targetCameraSize = 14f;
    Vector3 cameraNoShakePos;

    private void Awake()
    {
        instance = this;
        theCamera = GetComponent<Camera>();
        Cursor.lockState = CursorLockMode.Confined;
        cameraNoShakePos = transform.localPosition;
        targetCameraSize = theCamera.orthographicSize;
    }
    private void Start()
    {
        SetResolutionShakeMod();
        Cursor.lockState = CursorLockMode.Confined;
    }
    public void SetResolutionShakeMod()
    {
        //resolutionShakeMod = 0.6667f + ((float)(1920f * 1080f) / (theCamera.pixelWidth * theCamera.pixelHeight)) / 3f;
    }
    public void ShakeCameraImpulse(Vector3 p_dir, float p_strength)
    {
        float finalStrength = p_strength * shakeDurationMod;

        if (finalStrength > shakeTime)
        {
            shakeDir = p_dir;
            shakeDir.z = 0f;
            shakeDir.Normalize();
        }

        shakeTime = Mathf.Max(shakeTime + finalStrength, finalStrength);
        shakeTime *= 0.9f;

    }
    public void ManageCameraShake()
    {
        if (shakeTime > 0f)
        {
            cameraShakeAdd = (Mathf.PingPong(shakeTime * shakeSpeed, 1f) - 0.5f) * (Mathf.Pow(shakeTime + 1f, 0.5f) - 1f) * shakeDistanceMod * shakeDir;

            shakeTime -= Time.deltaTime;
            shakeTime *= 0.95f;
        }
        else
        {
            cameraShakeAdd = Vector3.Lerp(cameraShakeAdd, Vector3.zero, 0.5f);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            ShakeCameraImpulse(Random.onUnitSphere, 10f);
        }

        if (Mathf.Abs(Input.mouseScrollDelta.y) > 0.01f)
        {
            targetCameraSize = Mathf.Clamp(targetCameraSize + 2f * Mathf.Sign(-Input.mouseScrollDelta.y), minCameraSize, maxCameraSize);
        }

        theCamera.orthographicSize = SmoothFunc.Damp(theCamera.orthographicSize, targetCameraSize, 0.000001f, Time.unscaledDeltaTime);

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



            if (normalizedMousePositionOnScreen.x < 1.01f && normalizedMousePositionOnScreen.x > -0.01f && normalizedMousePositionOnScreen.y < 1.01f && normalizedMousePositionOnScreen.y > -0.01f)
            {
                if (normalizedMousePositionOnScreen.x <= 0.01f)
                    thisFrameMoveInput.x = -1f;
                else if (normalizedMousePositionOnScreen.x >= 0.99f)
                    thisFrameMoveInput.x = 1f;

                if (normalizedMousePositionOnScreen.y <= 0.01f)
                    thisFrameMoveInput.y = -1f;
                else if (normalizedMousePositionOnScreen.y >= 0.99f)
                    thisFrameMoveInput.y = 1f;
            }
            //thisFrameMoveInput.Normalize();

            SetCameraVelocity();
        }

        //theCamera.transform.Translate(cameraVelocity * Time.unscaledDeltaTime * movespeed, Space.Self);

        cameraNoShakePos += (Vector3)cameraVelocity * Time.unscaledDeltaTime * movespeed * theCamera.orthographicSize / 14f;
        ManageCameraShake();
        theCamera.transform.localPosition = cameraNoShakePos + cameraShakeAdd;

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
