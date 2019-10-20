using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTest : MonoBehaviour
{
    public Vector3 m_startingPos;
    private Rigidbody m_rb;
    public string m_collideTag = "Finish";

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody>();
        m_startingPos = transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == m_collideTag)
        {
            m_rb.velocity = Vector3.zero;
            m_rb.angularVelocity = Vector3.zero;
            transform.position = m_startingPos;
            transform.rotation = Quaternion.identity;
        }
    }
}
