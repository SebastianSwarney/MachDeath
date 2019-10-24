using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror.MachDeath
{
    public class WeaponDamagePlayers : NetworkBehaviour
    {
        public float m_spearDamage = 100;
        private ProjectileProperties m_spearProperties;

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
                        return;
                    }
                }
                
                hitHealth.TakeDamageSpear(m_spearDamage, m_spearProperties.m_spearOwner);
            }
        }


    }
}