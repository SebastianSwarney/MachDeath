using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearObject : MonoBehaviour
{
    public Transform m_rayOrgin;
    public float m_rayLength;

    public LayerMask m_mask;

    private Rigidbody m_body;

    private bool m_reflected;


    private void Start()
    {
        m_body = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Reflect();
    }

    private void Reflect()
    {
        RaycastHit hit;

        if (Physics.Raycast(m_rayOrgin.transform.position, m_rayOrgin.forward, out hit, m_rayLength, m_mask))
        {
            if (!m_reflected)
            {
                Vector3 reflectDir = Vector3.Reflect(m_rayOrgin.forward, hit.normal);

                //transform.position = hit.point + hit.normal * 5; 

                transform.rotation.SetFromToRotation(m_rayOrgin.forward, reflectDir);

                m_body.AddForce(m_rayOrgin.forward * 10, ForceMode.Impulse);

                //m_reflected = true;
            }


        }

        transform.LookAt(m_body.position + m_body.velocity);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(m_rayOrgin.transform.position, m_rayOrgin.forward * m_rayLength);
    }
}
