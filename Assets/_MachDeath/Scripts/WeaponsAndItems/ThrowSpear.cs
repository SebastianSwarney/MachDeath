using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror.MachDeath
{
    
    public class ThrowSpear : NetworkBehaviour
    {
        public GameObject m_spearObject;

        public float m_minSpeed, m_maxSpeed;
        private PlayerMovementController m_player;

        public Transform m_fireSpot;

        private PlayerProperties m_playerProperties;

        private ObjectPooler_Network m_spearPool;


        private void Start()
        {
            m_spearPool = ObjectPooler.instance.GetPooler(m_spearObject.name);
            m_playerProperties = GetComponent<PlayerProperties>();
            m_player = GetComponent<PlayerMovementController>();
        }

        public void SpawnSpear()
        {
            if (!isLocalPlayer) return;
            float percent = m_player.m_velocity.magnitude / m_player.m_maxMovementSpeed;

            CmdCreateSpear(m_fireSpot.position, m_fireSpot.rotation, m_fireSpot.forward, Mathf.Lerp(m_minSpeed, m_maxSpeed, percent));
        }
        [Command]
        public void CmdCreateSpear(Vector3 p_pos, Quaternion p_quat, Vector3 p_dir, float p_speed)
        {

            /*GameObject newSpear = m_spearPool.NewObject(p_pos);
            //GameObject newSpear = Instantiate(m_spearObject);
            newSpear.SetActive(true);
            newSpear.transform.position = p_pos;
            newSpear.transform.rotation = p_quat;
            
            newSpear.GetComponent<ProjectileProperties>().m_spearOwner = m_playerProperties;
            newSpear.GetComponent<PooledObject>().m_pooler = m_spearPool;
            NetworkServer.Spawn(newSpear, m_spearPool.assetId);
            newSpear.GetComponent<Rigidbody>().velocity =  p_dir* p_speed;*/
            RpcAssignVelocity(p_quat, p_pos, p_dir, p_speed);
        }

        [ClientRpc]
        private void RpcAssignVelocity(Quaternion p_quat, Vector3 p_pos, Vector3 p_dir, float p_speed)
        {
            GameObject newSpear = m_spearPool.NewObject(p_pos);
            //GameObject newSpear = Instantiate(m_spearObject);
            newSpear.SetActive(true);
            newSpear.transform.position = p_pos;
            newSpear.transform.rotation = p_quat;

            newSpear.GetComponent<ProjectileProperties>().m_spearOwner = m_playerProperties;
            newSpear.GetComponent<PooledObject>().m_pooler = m_spearPool;

            newSpear.GetComponent<ProjectileProperties>().m_spearOwner = m_playerProperties;
            newSpear.GetComponent<Rigidbody>().velocity = p_dir * p_speed;
            newSpear.GetComponent<PooledObject>().m_pooler = m_spearPool;
        }
        
    }
}