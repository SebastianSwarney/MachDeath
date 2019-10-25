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
        shieldData = GetComponentInParent<UtilityBase>();
        utilityBase = shieldData.GetComponentInParent<UtilityBase>().gameObject;
        shieldHealth = maxShieldHealth;
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
        ManualDamage();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == shieldData.spearTag)
        {
            Debug.Log("Shield Damaged " + spearDamage);
            DamageShield(spearDamage);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == shieldData.spearTag)
        {
            Debug.Log("Shield Damaged " + spearDamage);
            DamageShield(spearDamage);
        }
    }

    private void ManualDamage()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            DamageShield(spearDamage);
        }
    }

    void DamageShield(float damage)
    {
        Debug.Log("Shield took " + damage + "Damage!!!");
        shieldHealth -= damage;
    }

    void HealthCheck()
    {
        if (shieldHealth <= 0)
        {
            //Despawn Shield Right here
            Debug.Log("Shield Broken!!!");

            //Dont need all these below just need to tell weaponmanager to deactive everything
            //So the logic for reactivating and deactivng the shield can be handled purely by the WeaponManager
            shieldData.weaponController.DeActivateShield();
        }
    }
}
