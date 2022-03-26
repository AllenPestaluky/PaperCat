using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CatMovement : MonoBehaviour
{
    InputManager m_InputManager;
    Rigidbody m_RigidBody;

    enum MoveState 
    {
        Walk,
        Jump
    };
    MoveState m_MoveState;

    const float stickInputDeadzone = 0.1f;

    // So what do we need to do?
    // - Know our movement state.
    // - Know what input is coming in.
    // - Direct input to an appropriate component.

    // Start is called before the first frame update
    void Start()
    {
        m_InputManager = GetComponent<InputManager>();
        m_RigidBody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        HandleInput();        
    }

    void HandleInput()
    {
        // 1. See what input we have received.

        // Input from left stick or arrow keys.
        Vector2 leftStick = m_InputManager.GetLeftStickInput();
        bool bAnyMoveInput = (leftStick.magnitude > stickInputDeadzone);

        Vector3 moveInput;
        if (bAnyMoveInput)
        {
            moveInput = new Vector3(leftStick.x, 0.0f, leftStick.y);
        }
        else
        {
            moveInput = new Vector3(0.0f, 0.0f, 0.0f);
        }

        //bool jumpButtonDown = m_InputManager.GetSouthButtonDown(); // Pressed this frame.
        bool jumpButtonHeld = m_InputManager.GetSouthButton(); // Pressed or held.

        // 2. Decide what to do with the input.
        // May change states.
        // Then handle input based on state.
        // TODO: Make a state machine with an object for each state.

        switch (m_MoveState)
        {
            case MoveState.Walk:
            {
                HandleWalkMovement(moveInput, jumpButtonHeld);
            }
            break;

            case MoveState.Jump:
            {
                HandleJumpMovement(moveInput, jumpButtonHeld);
            }
            break;
        }
    }

    void HandleWalkMovement(Vector3 moveInput, bool jumpButtonHeld)
    {
        // Transition to jump state.
        if (jumpButtonHeld)
        {
            Debug.Log("Enter jump state");
            m_MoveState = MoveState.Jump;
            //HandleJumpMovement(moveInput, jumpButtonHeld);
            return;
        }

        // No transition, do walk movement.
        m_RigidBody.AddForce(moveInput, ForceMode.Impulse);
    }

    void HandleJumpMovement(Vector3 moveInput, bool jumpButtonHeld)
    {
        // Finish jump.
        if (!jumpButtonHeld)
        {
            // Release the jump
            Vector3 jumpForce = new Vector3(0.0f, 25.0f, 0.0f);
            m_RigidBody.AddForce(jumpForce, ForceMode.Impulse);

            // And for now, just back to walk state.
            m_MoveState = MoveState.Walk;
            Debug.Log("Release jump");

            return;
        }

        // TODO continue charging and aiming the jump.
    }
}

