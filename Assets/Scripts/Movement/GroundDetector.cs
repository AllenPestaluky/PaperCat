using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    [SerializeField]
    int m_GroundLayer = 8;

    public bool IsGrounded { get => m_IsGrounded; }
    bool m_IsGrounded = true;
    int m_GroundCount = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == m_GroundLayer)
        {
            m_GroundCount++;
            m_IsGrounded = m_GroundCount > 0;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == m_GroundLayer)
        {
            m_GroundCount--;
            m_IsGrounded = m_GroundCount > 0;
        }
    }
}
