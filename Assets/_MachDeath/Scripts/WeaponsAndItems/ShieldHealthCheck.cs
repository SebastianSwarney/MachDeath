using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldHealthCheck : MonoBehaviour
{
    private UtilityBase shieldData;

    public float shieldHealth, maxShieldHealth, spearDamage;

    public GameObject utilityBase;

    // Start is called before the first frame update
    void Start()
    {
        shieldData.GetComponentInParent<UtilityBase>();
        utilityBase = shieldData.GetComponentInParent<UtilityBase>().gameObject;
    }

    private void OnEnable()
    {
     
    }

    public void ResetHealth()
    {
        shieldHealth = maxShieldHealth;
    }

    // Update is called once per frame
    void Update()
    {
        HealthCheck();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == shieldData.spearTag)
        {
            Debug.Log("Shield Damaged " + spearDamage);
            DamageShield(spearDamage);
        }
    }

    void DamageShield(float damage)
    {
        shieldHealth -= damage;
    }

    void HealthCheck()
    {
        if (shieldHealth <= 0)
        {
            //Despawn Shield Right here
            Debug.Log("Shield Broken!!!");
            //Active Renegerate Cycle for Shield Before it comes back
            StartCoroutine(shieldData.GetComponentInParent<UtilityBase>().Countdown());
            utilityBase.SetActive(false);
        }
    }
}
