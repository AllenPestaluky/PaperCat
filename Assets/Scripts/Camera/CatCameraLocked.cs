using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatCameraLocked : MonoBehaviour
{
    [SerializeField, Range(0, 90)]
    [Tooltip("Angle camera will lock to in each direction.")]
    private int lookAngle = 15;

    [SerializeField, Range(0, 1)]
    [Tooltip("How long after camera angle change before accepting another. .")]
    private float secondsBetweenAngleChanges = 0.3f;

    [SerializeField, Range(0, 1)]
    [Tooltip("Threshold for when to count X/Y input.")]
    private float stickThreshold = 0.75f;

    [SerializeField]
    [Tooltip("Whether vertical look input is inverted.")]
    private bool invertVertical = false;

    [SerializeField]
    private Transform followTarget = null;

    enum LookDirection
    {
        Middle,
        Left,
        Right,
        Up,
        Down
    }

    private float angleChangeTimer = 0;
    private LookDirection currentLookDirection = LookDirection.Middle;

    public void ResetCamera()
    {
        // Start at middle
        currentLookDirection = LookDirection.Middle;
        UpdateFollowTarget();
    }

    public void UpdateCamera(Vector2 lookInput)
    {
        lookInput.Normalize();
        if (invertVertical)
        {
            lookInput.x *= -1;
        }

        angleChangeTimer += Time.deltaTime;

        if (angleChangeTimer > secondsBetweenAngleChanges)
        {
            bool hasChanged = false;

            // If stick pushed left
            if (lookInput.x < -stickThreshold)
            {
                hasChanged = LookLeft();
            }
            // If stick pushed right
            else if (lookInput.x > stickThreshold)
            {
                hasChanged = LookRight();
            }
            // If stick pushed forward
            else if (lookInput.y > stickThreshold)
            {
                hasChanged = LookUp();
            }
            // If stick pushed back
            else if (lookInput.y < -stickThreshold)
            {
                hasChanged = LookDown();
            }

            if (hasChanged)
            {
                UpdateFollowTarget();

                // Camera angle has changed, block any more for secondsBetweenAngleChanges
                angleChangeTimer = 0;
            }
        }
    }

    private void UpdateFollowTarget()
    {
        Debug.Log("UpdateFollowTarget: " + currentLookDirection);

        switch (currentLookDirection)
        {
            case LookDirection.Left:
                followTarget.localEulerAngles = new Vector3(0, -lookAngle, 0);
                break;
            case LookDirection.Right:
                followTarget.localEulerAngles = new Vector3(0, lookAngle, 0);
                break;
            case LookDirection.Up:
                followTarget.localEulerAngles = new Vector3(-lookAngle, 0, 0);
                break;
            case LookDirection.Down:
                followTarget.localEulerAngles = new Vector3(lookAngle, 0, 0);
                break;
            case LookDirection.Middle:
            default:
                followTarget.localEulerAngles = new Vector3(0, 0, 0);
                break;
        }
    }

    // Returns true if direction has changed.
    private bool LookLeft()
    {
        if (currentLookDirection == LookDirection.Left)
        {
            return false;
        }
        else if (currentLookDirection == LookDirection.Right)
        {
            currentLookDirection = LookDirection.Middle;
            return true;
        }
        else
        {
            currentLookDirection = LookDirection.Left;
            return true;
        }
    }

    // Returns true if direction has changed.
    private bool LookRight()
    {
        if (currentLookDirection == LookDirection.Right)
        {
            return false;
        }
        else if (currentLookDirection == LookDirection.Left)
        {
            currentLookDirection = LookDirection.Middle;
            return true;
        }
        else
        {
            currentLookDirection = LookDirection.Right;
            return true;
        }
    }

    // Returns true if direction has changed.
    private bool LookUp()
    {
        if (currentLookDirection == LookDirection.Up)
        {
            return false;
        }
        else if (currentLookDirection == LookDirection.Down)
        {
            currentLookDirection = LookDirection.Middle;
            return true;
        }
        else
        {
            currentLookDirection = LookDirection.Up;
            return true;
        }
    }

    // Returns true if direction has changed.
    private bool LookDown()
    {
        if (currentLookDirection == LookDirection.Down)
        {
            return false;
        }
        else if (currentLookDirection == LookDirection.Up)
        {
            currentLookDirection = LookDirection.Middle;
            return true;
        }
        else
        {
            currentLookDirection = LookDirection.Down;
            return true;
        }
    }
}
