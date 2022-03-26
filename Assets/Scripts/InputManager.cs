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

        return input;
    }

    public Vector2 GetRightStickInput()
    {
        Vector2 input = new Vector2(0.0f, 0.0f);

        Gamepad gp = Gamepad.current;
        if (gp != null)
        {
            input = gp.rightStick.ReadValue();
        }
        else
        {
            // TODO make camera with mouse not horrible
            //input = Mouse.current.delta.ReadValue();
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
