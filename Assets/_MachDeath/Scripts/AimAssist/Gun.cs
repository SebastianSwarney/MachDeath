using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{

    public GameObject m_bullet;
    
    public Transform m_fireSpot;
    public float m_bulletSpeed;
    private bool m_canFire = true;
    public float m_fireTime = .25f;
    private void Update()
    {
        if (Input.GetAxis("Fire3")>0)
        {
            if (m_canFire)
            {
                m_canFire = false;
                FireGun();
                
            }
        }
        else
        {
            m_canFire = true;
        }
    }

    private void FireGun()
    {
        GameObject bullet = Instantiate(m_bullet);
        bullet.transform.position = m_fireSpot.transform.position;
        bullet.transform.rotation = m_fireSpot.rotation;
        bullet.GetComponent<Rigidbody>().velocity = m_fireSpot.transform.forward * m_bulletSpeed;
    }


}
