using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPrediction : MonoBehaviour
{
    public LineRenderer arcRenderer;
    public Transform target;
    public Vector3 acceleration = new Vector3(0f, -9.8f, 0f);
    public float jumpHeight = 5f;

    void Update()
    {
        JumpArc arc = new JumpArc(transform.position, target.position, jumpHeight, acceleration);
        ShowJumpArc(arc);
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
}
