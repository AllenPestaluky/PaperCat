using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveStateWalk : MoveStateBase
{
    Rigidbody m_RigidBody;

    public new void Start()
    {
        base.Start();
        m_RigidBody = GetComponent<Rigidbody>();
    }

    public override CatMovement.EMoveState GetStateEnum()
    { 
        return CatMovement.EMoveState.Walk;
    }

    public override void OnMoveActionPerformed(Vector2 input)
    {
        Debug.Log("MoveStateBase Action Performed " + input.ToString());
        m_RigidBody.AddForce(GetLeftStickInput(input), ForceMode.Impulse);
    }

    private Vector3 GetLeftStickInput(Vector2 input)
    {
        float facing = Camera.main.transform.eulerAngles.y;
        return Quaternion.Euler(0, facing, 0) * new Vector3(input.x, 0, input.y);
    }

    public override void OnJumpStart()
    {
        m_CatMovement.ChangeState(CatMovement.EMoveState.Jump);
    }
}
