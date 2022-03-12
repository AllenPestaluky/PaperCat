using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public bool rotateForCamera = true;

    public Vector2 GetLeftStickInput()
    {
        Vector2 input = new Vector2(0.0f, 0.0f);

        Gamepad gp = Gamepad.current;
        if (gp != null)
        {
            input = gp.leftStick.ReadValue();
        }
        else
        {
            Keyboard kb = Keyboard.current;
            if (kb.aKey.isPressed)
            {
                input.x = -1.0f;
            }
            else if (kb.dKey.isPressed)
            {
                input.x = 1.0f;
            }

            if (kb.wKey.isPressed)
            {
                input.y = 1.0f;
            }
            else if (kb.sKey.isPressed)
            {
                input.y = -1.0f;
            }
        }


        if (rotateForCamera)
        {
            //TODO : gotta put the y input on z before rotating
            Debug.Log("input going in: " + input);
            float facing = Camera.main.transform.eulerAngles.y;
            Debug.Log("facing: " + facing);
            Vector3 rotatedInput = Quaternion.Euler(0, facing, 0) * new Vector3(input.x, 0, input.y);
            Debug.Log("input going out: " + input);
            input = new Vector2(rotatedInput.x, rotatedInput.z);
        }
        return input;
    }

    public bool GetSouthButton()
    {
        bool input = false;

        Gamepad gp = Gamepad.current;
        if (gp != null)
        {
            input = gp.buttonSouth.isPressed;
        }
        else
        {
            input = Keyboard.current.spaceKey.isPressed;
        }
        return input;
    }

    public bool GetSouthButtonDown()
    {
        bool input = false;
        Gamepad gp = Gamepad.current;
        if(gp != null)
        {
            input = gp.buttonSouth.wasPressedThisFrame;
        }
        else
        {
            input = Keyboard.current.spaceKey.wasPressedThisFrame;
        }

        return input;
    }

    public bool GetAnyLeftShoulder()
    {
        bool input = false;
        Gamepad gp = Gamepad.current;
        if (gp != null)
        {
            input = gp.leftTrigger.ReadValue() > 0 || gp.leftShoulder.ReadValue() > 0;
        }
        else
        {
            input = Keyboard.current.leftShiftKey.isPressed;
        }
        return input;
    }
}
