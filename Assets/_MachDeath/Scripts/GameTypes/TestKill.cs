using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror.MachDeath
{
    public class TestKill : NetworkBehaviour
    {
        public Transform m_fireSpot;
        public PlayerProperties m_playerProps;
        public KeyCode m_fireKeycode;
        
        private void Update()
        {
            if (!isLocalPlayer) return;
            Debug.DrawLine(m_fireSpot.position, m_fireSpot.position + m_fireSpot.forward * 100, Color.magenta);
            if (Input.GetKeyDown(m_fireKeycode))
            {
                CmdCheckSpear();   
            }

        }

        [Command]
        private void CmdCheckSpear()
        {
            RaycastHit hit;
            if (Physics.Raycast(m_fireSpot.transform.position, m_fireSpot.forward, out hit, Mathf.Infinity))
            {
                Debug.DrawLine(m_fireSpot.position, m_fireSpot.position + m_fireSpot.forward * 100, Color.red, 5f);
                Health hitHealth = hit.transform.GetComponent<Health>();
                if (hitHealth != null)
                {
                    
                    hitHealth.CmdTakeDamageSpear(10000000f, m_playerProps);
                }
            }
        }
    }
}