using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpeedBoostEvent : UnityEngine.Events.UnityEvent { }
namespace Mirror.MachDeath
{
    public class EnviromentalEffectsManager : EnviromentalBase
    {
        public SObj_EffectVariables m_effectVarSO;
        public GameObject m_Player;

        public LayerMask mask;
       
        public SpeedBoostEvent m_onSpeedBoost = new SpeedBoostEvent();

        //this would need to be the health value on the player controller script.
        //public float m_playerHealth;

        private void Start()
        {

        }


        private void Update()
        {

        }


        private void OnDrawGizmos()
        {
            if (m_effectVarSO.name == "HealthPickup")
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireCube(transform.position, new Vector3(5, 5, 5));

            }

            if (m_effectVarSO.name == "BoostPad")
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireCube(transform.position, new Vector3(5, 5, 5));

            }
        }



        public bool CheckCollisionLayer(LayerMask p_layerMask, GameObject p_collision)
        {
            if (p_layerMask == (p_layerMask | (1 << p_collision.gameObject.layer)))
            {
                return true;
            }
            else
            {
                return false;
            }

        }


        override public void OnTriggerEnter(Collider other)
        {
            if (CheckCollisionLayer(mask, m_Player))
            {
               
                if (m_effectVarSO.name == "BoostPad")
                {
                    Debug.Log("SHOULD BE BOOSTING");
                  
                    other.transform.GetComponent<PlayerMovementController>().SpeedBoost(m_effectVarSO.m_Force);
                    m_onSpeedBoost.Invoke();
                }

                if (m_effectVarSO.name == "HealthPickup")
                {
                    Debug.Log("HEALING");
                    other.transform.GetComponent<Health>().HealHealth(m_effectVarSO.m_HealthPickUpAmount);
                   
                }

            }
        }

        public void OnTriggerExit(Collider other)
        {
            if(CheckCollisionLayer(mask, m_Player))
            {
                if (m_effectVarSO.name == "BoostPad")
                {
                    Debug.Log("SHOULD BE BOOSTING");

                    other.transform.GetComponent<PlayerMovementController>().SpeedBoost(0);
                    m_onSpeedBoost.Invoke();
                }

            }
        }

    }
}