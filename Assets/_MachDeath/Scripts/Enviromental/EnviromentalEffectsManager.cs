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


        override public void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
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

    }
}