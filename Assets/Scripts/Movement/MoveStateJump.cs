using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveStateJump : MoveStateBase
{
    public float TapJumpDistance = 4f;
    public float TapJumpHeigh = 4f;

    Rigidbody m_RigidBody;
    LineRenderer m_ArcRenderer;

    float m_Time = 0f;
    JumpArc m_JumpArc;

    public new void Start()
    {
        base.Start();
        m_RigidBody = GetComponent<Rigidbody>();
        m_ArcRenderer = GetComponent<LineRenderer>();
        HideJumpArc();
    }

    public override CatMovement.EMoveState GetStateEnum()
    { 
        return CatMovement.EMoveState.Jump;
    }

    public override void Enter()
    {
        m_Time = 0f;
        m_JumpArc = new JumpArc(transform.position, transform.position + transform.forward * TapJumpDistance, TapJumpHeigh, Physics.gravity);
        ShowJumpArc(m_JumpArc);
    }

    public override void ActiveStateFixedUpdate(float deltaTime)
    {
        m_Time += deltaTime;
    }

    public override void OnJumpHoldReleased(double duration) 
    {
        m_RigidBody.AddForce(m_JumpArc.InitialVelocity, ForceMode.VelocityChange);
    }

    void ShowJumpArc(JumpArc arc)
    {
        int segments = m_ArcRenderer.positionCount;
        Vector3[] positions = new Vector3[segments];
        float tf = arc.tf;
        for (int i = 0; i < segments; i++)
        {
            positions[i] = arc.GetPositionAlong(i * tf / (segments - 1));
        }
        m_ArcRenderer.SetPositions(positions);
        m_ArcRenderer.enabled = true;
    }

    void HideJumpArc()
    {
        m_ArcRenderer.enabled = false;
    }
}
