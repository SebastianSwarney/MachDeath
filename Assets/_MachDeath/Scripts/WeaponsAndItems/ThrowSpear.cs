using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror.MachDeath
{
    public class ThrowSpear : NetworkBehaviour
    {
        public GameObject m_spearObject;

        public float m_throwSpeed;
        public Transform m_fireSpot;

        private PlayerProperties m_playerProperties;
        private void Start()
        {
            m_playerProperties = GetComponent<PlayerProperties>();
            print("Make Spear");
        }
        [Command]
        public void CmdCreateSpear()
        {
            
            GameObject newSpear = Instantiate(m_spearObject);
            newSpear.transform.position = m_fireSpot.position;
            newSpear.transform.rotation = m_fireSpot.rotation;
            
            newSpear.GetComponent<ProjectileProperties>().m_spearOwner = m_playerProperties;
            print("Make Spear");
            NetworkServer.Spawn(newSpear);

            RpcAssignVelocity(newSpear);
        }

        [ClientRpc]
        private void RpcAssignVelocity(GameObject p_spear)
        {
            p_spear.GetComponent<ProjectileProperties>().m_spearOwner = m_playerProperties;
            p_spear.GetComponent<Rigidbody>().velocity = m_fireSpot.forward * m_throwSpeed;
        }
        
    }
}