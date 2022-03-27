using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CatCamera : MonoBehaviour
{
    [SerializeField]
    private float rotationPower = 3f;

    [SerializeField]
    private Transform followTransform = null;

    [SerializeField]
    private InputManager inputManager = null;

    bool isCameraLocked = false;
    private Vector2 _look;

    public void SetCameraLocked(bool isLocked)
    {
        // TODO actually do something with this
        isCameraLocked = isLocked;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Debug.Log("cursor locked");
    }
    
    private void Update()
    {
        _look = inputManager.GetRightStickInput();
        Debug.Log("MouseX: " + _look.x + " MouseY: " + _look.y);

        // Based on https://www.youtube.com/watch?v=537B1kJp9YQ

        // Rotate the Follow Target transform based on the input
        followTransform.rotation *= Quaternion.AngleAxis(_look.x * rotationPower, Vector3.up);
        followTransform.rotation *= Quaternion.AngleAxis(_look.y * rotationPower, Vector3.right);

        // Clamp the Up/Down rotation
        var angles = followTransform.localEulerAngles;
        angles.z = 0;
        if (angles.x > 180 && angles.x < 340)
        {
            angles.x = 340;
        }
        else if (angles.x < 180 && angles.x > 40)
        {
            angles.x = 40;
        }
        followTransform.transform.localEulerAngles = angles;
    }
}
