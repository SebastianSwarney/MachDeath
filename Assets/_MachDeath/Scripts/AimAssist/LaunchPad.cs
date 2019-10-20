using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchPad : MonoBehaviour
{
    public float m_force = 15;

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb!= null)
        {
            rb.velocity = Vector3.zero;
            rb.AddForce(transform.forward * m_force,ForceMode.Impulse);
        }
        
    }


}
