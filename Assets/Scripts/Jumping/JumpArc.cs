using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public struct JumpArc
{
    Vector3 pi;
    Vector3 pf;
    Vector3 acc;

    Vector3 vi;
    public Vector3 InitialVelocity { get => vi; }
    public float tf { get; private set; }

    public JumpArc(Vector3 initialPositon, Vector3 targetPosition, float jumpHeight, Vector3 acceleration)
    {
        pi = initialPositon;
        pf = targetPosition;
        acc = acceleration;

        vi.y = Mathf.Sqrt(-2 * acc.y * jumpHeight); //Find initial y velocity for target height
        tf = QuadraticSolver(0.5f * acc.y, vi.y, pi.y - pf.y); //Use initial velocity in y to find time at target y

        // Use the final time to find the inital velocity in x and z
        vi.x = (pf.x - pi.x) / tf - 0.5f * acc.x * tf;
        vi.z = (pf.z - pi.z) / tf - 0.5f * acc.z * tf;
    }

    static private float QuadraticSolver(float a, float b, float c)
    {
        float s1 = (-b + Mathf.Sqrt(b * b - 4 * a * c)) / (2 * a);
        float s2 = (-b - Mathf.Sqrt(b * b - 4 * a * c)) / (2 * a);
        return s1 > s2 ? s1 : s2;
    }

    public Vector3 GetPositionAlong(float t)
    {
        return pi + vi * t + 0.5f * acc * t * t;
    }
}
