using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveStateJump : MoveStateBase
{
    Rigidbody m_RigidBody;

    public new void Start()
    {
        base.Start();
        m_RigidBody = GetComponent<Rigidbody>();
    }

    public override CatMovement.EMoveState GetStateEnum()
    { 
        return CatMovement.EMoveState.Jump;
    }

    public override void OnJumpActionCanceled() 
    {
        //Debug.Log("MoveStateBase Action Canceled");
        Vector3 jumpForce = new Vector3(0.0f, 25.0f, 0.0f);
        m_RigidBody.AddForce(jumpForce, ForceMode.Impulse);
    }
}
