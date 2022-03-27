using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveStateWalk : MoveStateBase
{
    Rigidbody m_RigidBody;
    public MoveStateWalk(CatMovement cm, string name) : base(cm, name)
    {
        m_RigidBody = cm.GetComponent<Rigidbody>();
    }

    public override void OnMoveActionCanceled() 
    {
        Debug.Log("MoveStateBase Action Canceled");
    }

    public override void OnMoveActionPerformed(Vector2 input)
    {
        Debug.Log("MoveStateBase Action Performed " + input.ToString());
        m_RigidBody.AddForce(GetLeftStickInput(input), ForceMode.Impulse);
    }

    public override void OnMoveActionStarted() 
    {
        Debug.Log("MoveStateBase Action Started");
    }

    private Vector3 GetLeftStickInput(Vector2 input)
    {
        float facing = Camera.main.transform.eulerAngles.y;
        return Quaternion.Euler(0, facing, 0) * new Vector3(input.x, 0, input.y);
    }

    public override void OnJumpActionStarted()
    {
        m_CatMovement.ChangeState(CatMovement.EMoveState.Jump);
    }
}
