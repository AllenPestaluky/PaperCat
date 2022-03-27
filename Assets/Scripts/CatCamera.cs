using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class CatCamera : MonoBehaviour
{
    [SerializeField]
    private float rotationPower = 3f;

    [SerializeField, Range(0, 90)]
    [Tooltip("Angle camera will lock to in each direction.")]
    private int lockedCamLookAngle = 15;

    [SerializeField, Range(0, 1)]
    [Tooltip("Threshold for when to count X/Y input in locked camera state.")]
    private float lockedCamStickThreshold = 0.75f;

    [SerializeField, Range(0, 1)]
    [Tooltip("How long after letting go of the control before returning to the middleLockedFollowTransform in locked camera state..")]
    private float lockedCamTimeUntilReturn = 0.1f;

    [SerializeField]
    private CinemachineVirtualCamera freeCamera = null;

    [SerializeField]
    private CinemachineVirtualCamera lockedCamera = null;

    [SerializeField]
    private Transform freeFollowTransform = null;

    [SerializeField]
    private Transform lockedFollowTransform = null;

    [SerializeField]
    private InputManager inputManager = null;

    private bool isCameraLocked = false;
    private float lockedCameraReturnTimer = 0;

    public void SetCameraLocked(bool isLocked)
    {
        if (isCameraLocked != isLocked)
        {
            isCameraLocked = isLocked;
            freeCamera.gameObject.SetActive(!isCameraLocked);
            lockedCamera.gameObject.SetActive(isCameraLocked);

            if (!isCameraLocked)
            {
                // Start freelook at 0,0,0 again
                freeFollowTransform.localEulerAngles = new Vector3(0, 0, 0);
            }
        }
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        SetCameraLocked(false);
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
            UpdateLockedCamera(lookInput);
        }
        else
        {
            UpdateFreeCamera(lookInput);
        }
    }

    // Camera can only look forward left, forward right, or forward
    private void UpdateLockedCamera(Vector2 lookInput)
    {
        // If stick pushed left
        if (lookInput.x < -lockedCamStickThreshold)
        {
            lockedFollowTransform.localEulerAngles = new Vector3(0, -lockedCamLookAngle, 0);
            lockedCameraReturnTimer = 0;
        }
        // If stick pushed right
        else if (lookInput.x > lockedCamStickThreshold)
        {
            lockedFollowTransform.localEulerAngles = new Vector3(0, lockedCamLookAngle, 0);
            lockedCameraReturnTimer = 0;
        }
        // If stick pushed forward
        else if (lookInput.y > lockedCamStickThreshold)
        {
            lockedFollowTransform.localEulerAngles = new Vector3(lockedCamLookAngle, 0, 0);
            lockedCameraReturnTimer = 0;
        }
        // If stick pushed back
        else if (lookInput.y < -lockedCamStickThreshold)
        {
            lockedFollowTransform.localEulerAngles = new Vector3(-lockedCamLookAngle, 0, 0);
            lockedCameraReturnTimer = 0;
        }
        // If stick not pushed
        else
        {
            // The stick input often returns (0,0) even when it's held left or right, 
            // so we need to wait a few frames before acting on it.
            lockedCameraReturnTimer += Time.deltaTime;

            if (lockedCameraReturnTimer > lockedCamTimeUntilReturn)
            {
                lockedFollowTransform.localEulerAngles = new Vector3(0, 0, 0);
                lockedCameraReturnTimer = 0;
            }
        }
    }

    // Free look camera
    private void UpdateFreeCamera(Vector2 lookInput)
    {
        // Based on https://www.youtube.com/watch?v=537B1kJp9YQ

        // Rotate the Follow Target transform based on the input
        freeFollowTransform.rotation *= Quaternion.AngleAxis(lookInput.x * rotationPower, Vector3.up);
        freeFollowTransform.rotation *= Quaternion.AngleAxis(lookInput.y * rotationPower, Vector3.right);

        // Clamp the Up/Down rotation
        var angles = freeFollowTransform.localEulerAngles;
        angles.z = 0;
        if (angles.x > 180 && angles.x < 340)
        {
            angles.x = 340;
        }
        else if (angles.x < 180 && angles.x > 40)
        {
            angles.x = 40;
        }
        freeFollowTransform.transform.localEulerAngles = angles;
    }
}
