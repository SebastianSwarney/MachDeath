using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBoundary : MonoBehaviour
{
    
    private void OnTriggerEnter(Collider collision)
    {
        Health health = collision.gameObject.GetComponent<Health>();
        if (health != null)
        {
            print("Take Damage");
            health.TakeDamage(100000000000f);
        }
    }


}
