using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveStateJump : MoveStateBase
{
    Rigidbody m_RigidBody;
    public MoveStateJump(CatMovement cm, string name) : base(cm, name)
    {
        m_RigidBody = cm.GetComponent<Rigidbody>();
    }

    public override void OnJumpActionCanceled() 
    {
        //Debug.Log("MoveStateBase Action Canceled");
        Vector3 jumpForce = new Vector3(0.0f, 25.0f, 0.0f);
        m_RigidBody.AddForce(jumpForce, ForceMode.Impulse);
    }
}
