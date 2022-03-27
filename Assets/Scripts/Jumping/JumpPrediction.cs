using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JumpPrediction : MonoBehaviour
{
    public enum JumpProcessingType
    {
        ManualJump,
        UnityJump
    }

    public LineRenderer arcRenderer;
    public float jumpHeight = 3f;
    public float jumpDistance = 4f; 

    public JumpProcessingType jumpProcessingType = JumpProcessingType.ManualJump;

    // Manual movement handling
    bool isJumping = false;
    float jumpTime;
    JumpArc jumpArc;

    void Update()
    {
        if (Keyboard.current.ctrlKey.wasPressedThisFrame)
        {
            TapJump();
        }
    }

    private void FixedUpdate()
    {
        switch (jumpProcessingType)
        {
            case JumpProcessingType.ManualJump:
                if (isJumping)
                {
                    Rigidbody rb = GetComponent<Rigidbody>();
                    rb.MovePosition(jumpArc.GetPositionAlong(jumpTime));
                    jumpTime += Time.fixedDeltaTime;
                }
                break;
            case JumpProcessingType.UnityJump:
                break;
        }
    }

    public void ShowJumpArc(JumpArc arc)
    {
        int segments = arcRenderer.positionCount;
        Vector3[] positions = new Vector3[segments];
        float tf = arc.tf;
        for (int i = 0; i < segments; i++)
        {
            positions[i] = arc.GetPositionAlong(i * tf / (segments - 1));
        }
        arcRenderer.SetPositions(positions);
    }

    public void TapJump()
    {
        Vector3 target = transform.position + transform.forward * jumpDistance;
        jumpArc = new JumpArc(transform.position, target, jumpHeight, Physics.gravity);
        ShowJumpArc(jumpArc);

        Rigidbody rb = GetComponent<Rigidbody>();
        switch (jumpProcessingType)
        {
            case JumpProcessingType.ManualJump:
                jumpTime = 0f;
                isJumping = true;
                rb.useGravity = false;
                rb.velocity = Vector3.zero;
                break;
            case JumpProcessingType.UnityJump:
                rb.AddForce(jumpArc.InitialVelocity, ForceMode.VelocityChange);
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (jumpProcessingType)
        {
            case JumpProcessingType.ManualJump:
                if (isJumping)
                {
                    isJumping = false;
                    GetComponent<Rigidbody>().useGravity = true;
                }
                break;
        }
    }
}
