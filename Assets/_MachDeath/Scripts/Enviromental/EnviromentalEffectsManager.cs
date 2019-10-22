using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpeedBoostEvent : UnityEngine.Events.UnityEvent { }
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
        if(m_effectVarSO.name == "HealthPickup")
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position, new Vector3(5,5,5));

        }

        if (m_effectVarSO.name == "BoostPad")
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(transform.position, new Vector3(5, 5, 5));

        }
    }


    override public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if(m_effectVarSO.name == "BoostPad")
            {
                Debug.Log("SHOULD BE BOOSTING");
                m_Player.GetComponent<Rigidbody>().AddForce(m_Player.transform.forward * m_effectVarSO.m_Force, ForceMode.Impulse);
                m_onSpeedBoost.Invoke();
            }

            if(m_effectVarSO.name == "HealthPickup")
            {
                //TODO: get the playcontroller scripts health value and add to it if it is below 100%.
                Debug.Log("HEALING");
                m_Player.GetComponent<Health>().HealHealth(m_effectVarSO.m_HealthPickUpAmount);
                //m_playerHealth += m_effectVarSO.m_HealthPickUpAmount; 

            }

            

        }
    }

}
