using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror.MachDeath{
    public class KillBoundary : NetworkBehaviour
    {

        private void OnTriggerEnter(Collider collision)
        {
            Health health = collision.gameObject.GetComponent<Health>();
            if (health != null)
            {

                    health.TakeDamage(100000000000f);
                
                
            }
        }


    }
}
