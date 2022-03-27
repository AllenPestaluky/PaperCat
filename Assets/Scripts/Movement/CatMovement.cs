using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class CatMovement : MonoBehaviour
{
    [SerializeField]
    InputActionAsset m_CatActionMap;

    [SerializeField]
    GroundDetector m_GroundedDetector;

    // State stuff
    public enum EMoveState 
    {
        Invalid,
        Walk,
        Run,
        Jump,
    };
    Dictionary<EMoveState, MoveStateBase> m_MoveStateMap;

    [SerializeField]
    MoveStateBase m_CurrentState;

    void Start()
    {
        Debug.Assert(m_CatActionMap != null);

        m_MoveStateMap = new Dictionary<EMoveState, MoveStateBase>();
        foreach (MoveStateBase state in GetComponents<MoveStateBase>())
        {
            m_MoveStateMap.Add(state.GetStateEnum(), state);
        }

        if (m_CurrentState == null)
        {
            if (m_MoveStateMap.ContainsKey(EMoveState.Walk))
            {
                m_CurrentState = m_MoveStateMap[EMoveState.Walk];
            }
            else
            {
                Debug.LogError("CatMovement - Default state not found. " +
                    "Attach a MoveStateWalk component, or set another state as the default.");
            }
        }

        m_CatActionMap.Enable();

        // refactor: macro or helper takes an action name and binds ???
        InputAction moveAction = m_CatActionMap.FindAction("Move");
        moveAction.started += OnMoveActionStarted;
        moveAction.performed += OnMoveActionPerformed;
        moveAction.canceled += OnMoveActionCanceled;

        InputAction jumpAction = m_CatActionMap.FindAction("Jump");
        jumpAction.started += OnJumpActionStarted;
        jumpAction.performed += OnJumpActionPerformed;
        jumpAction.canceled += OnJumpActionCanceled;
    }

    private void OnDestroy()
    {
        InputAction moveAction = m_CatActionMap.FindAction("Move");
        moveAction.started -= OnMoveActionStarted;
        moveAction.performed -= OnMoveActionPerformed;
        moveAction.canceled -= OnMoveActionCanceled;

        InputAction jumpAction = m_CatActionMap.FindAction("Jump");
        jumpAction.started -= OnJumpActionStarted;
        jumpAction.performed -= OnJumpActionPerformed;
        jumpAction.canceled -= OnJumpActionCanceled;
    }

    void Update()
    {
        m_CurrentState.ActiveStateUpdate(Time.deltaTime);
    }

    void FixedUpdate()
    {
        m_CurrentState.ActiveStateFixedUpdate(Time.fixedDeltaTime);
    }

    public void ChangeState(EMoveState newMoveState)
    {
        m_CurrentState.Exit();
        m_CurrentState = m_MoveStateMap[newMoveState];
        m_CurrentState.Enter();
    }

    void OnMoveActionStarted(InputAction.CallbackContext context)
    {
        m_CurrentState.OnMoveActionStarted();
    }

    void OnMoveActionPerformed(InputAction.CallbackContext context)
    {
        Vector2 input = context.action.ReadValue<Vector2>();
        m_CurrentState.OnMoveActionPerformed(input);
    }

    void OnMoveActionCanceled(InputAction.CallbackContext context)
    {
        m_CurrentState.OnMoveActionCanceled();
    }

    void OnJumpActionStarted(InputAction.CallbackContext context)
    {
        m_CurrentState.OnJumpActionStarted();
    }

    void OnJumpActionPerformed(InputAction.CallbackContext context)
    {
        m_CurrentState.OnJumpActionPerformed();
    }

    void OnJumpActionCanceled(InputAction.CallbackContext context)
    {
        m_CurrentState.OnJumpActionCanceled();
    }

}

