using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchTarget : MonoBehaviour
{
    public string m_collideTag;
    private Rigidbody rb;
    private Vector3 startingPos;
    private void Awake()
    {
        startingPos = transform.position;
        rb = GetComponent<Rigidbody>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == m_collideTag)
        {
            ResetMe();
        }
    }
    void ResetMe()
    {
        transform.position = startingPos;
        rb.velocity = Vector3.zero;
        transform.rotation = Quaternion.identity;
    }
}
