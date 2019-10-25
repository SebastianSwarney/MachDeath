using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror.MachDeath
{
    [System.Serializable]
    public class SpearCollisionEvents : UnityEngine.Events.UnityEvent { }
    
    public class WeaponDamagePlayers : NetworkBehaviour
    {
        public float m_spearDamage = 100;
        private ProjectileProperties m_spearProperties;

        public SpearEvents m_spearCollisionEvents;
        [System.Serializable]
        public struct SpearEvents
        {
            public SpearCollisionEvents m_spearCollisionEvent;
            public SpearCollisionEvents m_destroySpearEvent;
        }

        private void Start()
        {
            m_spearProperties = GetComponent<ProjectileProperties>();
            
        }
        private void OnTriggerEnter(Collider other)
        {
            
            Health hitHealth = other.transform.GetComponent<Health>();
            if (hitHealth != null)
            {
                
                PlayerProperties playerProps = other.transform.GetComponent<PlayerProperties>();
                if (playerProps != null)
                {
                    if (playerProps == m_spearProperties.m_spearOwner)
                    {
                        m_spearCollisionEvents.m_destroySpearEvent.Invoke();
                        return;
                    }
                }
                
                hitHealth.TakeDamageSpear(m_spearDamage, m_spearProperties.m_spearOwner);
                m_spearCollisionEvents.m_destroySpearEvent.Invoke();

            }
        }




    }
}