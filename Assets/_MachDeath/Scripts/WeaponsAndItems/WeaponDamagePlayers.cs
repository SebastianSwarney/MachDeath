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
        private void OnCollisionEnter(Collision collision)
        {
            Health hitHealth = collision.transform.GetComponent<Health>();
            if (hitHealth != null)
            {
                hitHealth.TakeDamageSpear(m_spearDamage, m_spearProperties.m_spearOwner);
            }

        }


    }
}