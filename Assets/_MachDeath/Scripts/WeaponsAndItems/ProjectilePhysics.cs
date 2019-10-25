using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePhysics : MonoBehaviour
{
    private Rigidbody m_rb;
    public LayerMask m_collisionMask;
    public float m_sphereCastRadius;


    [Header("Debugging")]
    public bool m_debugging;
    public Color m_debuggingColor;

    private void Start()
    {
        m_rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.SphereCast(m_rb.position, m_sphereCastRadius, m_rb.velocity, out hit, m_rb.velocity.magnitude, m_collisionMask))
        {
            float distance = (hit.point - m_rb.position).magnitude;
            m_rb.position = (m_rb.velocity.normalized * distance) + m_rb.position;
            print("Hit");
        }
    }

    private void OnDrawGizmos()
    {
        if (!m_debugging) return;
        Gizmos.color = m_debuggingColor;
        Gizmos.DrawWireSphere(transform.position, m_sphereCastRadius);
    }
}
