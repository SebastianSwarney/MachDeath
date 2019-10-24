using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror.MachDeath
{
    public class ThrowSpearAuto : MonoBehaviour
    {
        public GameObject m_spearObject;

        public float m_minSpeed, m_maxSpeed;
        private PlayerMovementController m_player;

        public Transform m_fireSpot;

        private PlayerProperties m_playerProperties;
        private void Start()
        {
            m_playerProperties = GetComponent<PlayerProperties>();
            m_player = GetComponent<PlayerMovementController>();
        }

        public void SpawnSpear()
        {
            //float percent = m_player.m_velocity.magnitude / m_player.m_maxMovementSpeed;

            CmdCreateSpear(m_fireSpot.position, m_fireSpot.rotation, m_fireSpot.forward, m_maxSpeed);
        }

        [Command]
        public void CmdCreateSpear(Vector3 p_pos, Quaternion p_quat, Vector3 p_dir, float p_speed)
        {

            GameObject newSpear = Instantiate(m_spearObject);
            newSpear.transform.position = p_pos;
            newSpear.transform.rotation = p_quat;

            newSpear.GetComponent<ProjectileProperties>().m_spearOwner = m_playerProperties;
            NetworkServer.Spawn(newSpear);
            newSpear.GetComponent<Rigidbody>().velocity = p_dir * p_speed;
            RpcAssignVelocity(newSpear, p_quat, p_pos, p_dir, p_speed);
        }

        [ClientRpc]
        private void RpcAssignVelocity(GameObject p_spear, Quaternion p_quat, Vector3 p_pos, Vector3 p_dir, float p_speed)
        {
            p_spear.GetComponent<ProjectileProperties>().m_spearOwner = m_playerProperties;
            p_spear.GetComponent<Rigidbody>().velocity = p_dir * p_speed;
        }

    }
}
