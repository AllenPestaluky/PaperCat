using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class CatCamera : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera virtualCameraFree = null;

    [SerializeField]
    private CinemachineVirtualCamera virtualCameraLocked = null;

    private CatCameraFree catCameraFree = null;
    private CatCameraLocked catCameraLocked = null;
    private InputManager inputManager = null;

    private bool isCameraLocked = false;

    public void SetCameraLocked(bool isLocked)
    {
        if (isCameraLocked != isLocked)
        {
            isCameraLocked = isLocked;

            if (isCameraLocked)
            {
                catCameraLocked.ResetCamera();
            }
            else
            {
                catCameraFree.ResetCamera();
            }

            virtualCameraFree.gameObject.SetActive(!isCameraLocked);
            virtualCameraLocked.gameObject.SetActive(isCameraLocked);
        }
    }

    private bool hasSetInitialState = false;

    private void Start()
    {
        catCameraFree = GetComponent<CatCameraFree>();
        catCameraLocked = GetComponent<CatCameraLocked>();
        inputManager = GetComponent<InputManager>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        // TODO using left shoulder is just for debug
        if (inputManager.GetAnyLeftShoulderDown())
        {
            SetCameraLocked(!isCameraLocked);
        }

        // TODO eventually CatMovement should be calling this I believe
        UpdateCamera(inputManager.GetRightStickInput());
    }

    public void UpdateCamera(Vector2 lookInput)
    {
        if (isCameraLocked)
        {
            catCameraLocked.UpdateCamera(lookInput);
        }
        else
        {
            catCameraFree.UpdateCamera(lookInput);
        }
    }
}
