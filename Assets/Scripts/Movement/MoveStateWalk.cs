using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveStateWalk : MoveStateBase
{
    Rigidbody m_RigidBody;

    [SerializeField]
    [Tooltip("Max speed in units per second")]
    float Speed = 1.0f;
    [SerializeField]
    [Tooltip("Acceleration in units per second per second")]
    float Acceleration = 4.0f;
    [SerializeField]
    [Tooltip("Deceleration in units per second per second")]
    float Deceleration = 10.0f;
    [SerializeField]
    [Tooltip("Rotation speed, in radians per second")]
    float RotationSpeed = 6.28318f;

    private Vector2 m_StickDirection = Vector2.zero;
    private Vector3 m_CurrentDirection;
    private float m_CurrentSpeed = 0.0f;

    public new void Start()
    {
        base.Start();
        m_RigidBody = GetComponent<Rigidbody>();

        m_CurrentDirection = m_RigidBody.rotation * Vector3.forward;
    }

    public override CatMovement.EMoveState GetStateEnum()
    { 
        return CatMovement.EMoveState.Walk;
    }

    public override void OnMoveActionPerformed(Vector2 input)
    {
        m_StickDirection = input;
    }

    public override void OnMoveActionCanceled()
    {
        m_StickDirection = Vector2.zero;
    }

    public override void ActiveStateFixedUpdate(float deltaTime)
    {
        Vector3 stickMovementDirection = GetLeftStickInput(m_StickDirection);

        float prevSpeed = m_CurrentSpeed;
        float desiredSpeed = stickMovementDirection.magnitude * Speed;

        if (stickMovementDirection.magnitude > 0.1f)
        {
            m_CurrentDirection = Vector3.RotateTowards(m_CurrentDirection, stickMovementDirection, RotationSpeed * deltaTime, 0.0f);
        }
        else
        {
            desiredSpeed = 0.0f;
        }

        if (prevSpeed < desiredSpeed)
        {
            m_CurrentSpeed += Acceleration * deltaTime;
        }
        if (prevSpeed > desiredSpeed)
        {
            m_CurrentSpeed -= Deceleration * deltaTime;
        }

        if (m_CurrentSpeed > Speed)
        {
            m_CurrentSpeed = Speed;
        }

        if (m_CurrentSpeed < 0.0f)
        {
            m_CurrentSpeed = 0.0f;
        }

        m_RigidBody.rotation = Quaternion.LookRotation(m_CurrentDirection);

        Vector3 finalVelocity = m_CurrentSpeed * m_CurrentDirection;
        m_RigidBody.velocity = new Vector3(finalVelocity.x, m_RigidBody.velocity.y, finalVelocity.z);
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
