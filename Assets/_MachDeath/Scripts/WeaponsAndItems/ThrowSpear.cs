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
        }

        public void SpawnSpear()
        {

            CmdCreateSpear(m_fireSpot.position, m_fireSpot.rotation, m_fireSpot.forward);
        }
        [Command]
        public void CmdCreateSpear(Vector3 p_pos, Quaternion p_quat, Vector3 p_dir)
        {
            
            GameObject newSpear = Instantiate(m_spearObject);
            newSpear.transform.position = p_pos;
            newSpear.transform.rotation = p_quat;
            
            newSpear.GetComponent<ProjectileProperties>().m_spearOwner = m_playerProperties;
            NetworkServer.Spawn(newSpear);
            newSpear.GetComponent<Rigidbody>().velocity =  p_dir* m_throwSpeed;
            RpcAssignVelocity(newSpear,p_quat, p_pos, p_dir);
        }

        [ClientRpc]
        private void RpcAssignVelocity(GameObject p_spear, Quaternion p_quat, Vector3 p_pos, Vector3 p_dir)
        {
            p_spear.GetComponent<ProjectileProperties>().m_spearOwner = m_playerProperties;
            p_spear.GetComponent<Rigidbody>().velocity = p_dir * m_throwSpeed;
        }
        
    }
}