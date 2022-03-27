using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveStateBase : MonoBehaviour
{
    protected CatMovement m_CatMovement;

    public void Start()
    {
        m_CatMovement = GetComponent<CatMovement>();
    }

    public virtual CatMovement.EMoveState GetStateEnum() { return CatMovement.EMoveState.Invalid; }

    public virtual void Enter() { }

    public virtual void Exit() { }

    public virtual void ActiveStateUpdate(float deltaTime) { }

    public virtual void ActiveStateFixedUpdate(float deltaTime) { }

    public virtual void OnMoveActionStarted() { }

    public virtual void OnMoveActionPerformed(Vector2 input) { }

    public virtual void OnMoveActionCanceled() { }

    public virtual void OnJumpActionStarted() { }

    public virtual void OnJumpActionPerformed() { }

    public virtual void OnJumpActionCanceled() { }


}
