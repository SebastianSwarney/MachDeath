using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class HealthEvent : UnityEngine.Events.UnityEvent { }
public class Health : MonoBehaviour
{
    #region Generic Health Values
    public float m_maxHealth;
    [HideInInspector]
    public float m_currentHealth;
    [HideInInspector]
    public bool m_isDead;
    public HealthEvent m_onDied = new HealthEvent();
    
    #endregion

    #region Shield Values
    [Header("Shields")]
    public bool m_useShields = false;
    public float m_shieldDamageMultiplier = .75f;
    public float m_maxShieldStrength;
    [HideInInspector]
    public float m_currentShieldStrength;
    public bool m_shieldRegeneration = true;
    public float m_shieldRegenDelay;
    public float m_shieldRegenTimeToFull;
    private float m_shieldRegenCurrentTime;
    private Coroutine m_shieldRegenerationCoroutine;
    private WaitForSeconds m_shieldRegenDelayTimer;
    #endregion

    #region Health Regeneration Values
    [Header("Health Regeneration")]
    public bool m_useHealthRegeneration = false;
    public float m_maxHealthRegenerationAmount;
    public float m_healthRegnerationDelay;
    public float m_healthRegenTimeToFull;
    private float m_healthRegenCurrentTime;
    private Coroutine m_healthRegenCorotine;
    private WaitForSeconds m_healthRegenDelayTimer;
    #endregion

    private void Start()
    {
        m_shieldRegenDelayTimer = new WaitForSeconds(m_shieldRegenDelay);
        m_healthRegenDelayTimer = new WaitForSeconds(m_healthRegnerationDelay);
        Respawn();
    }

    public void Respawn()
    {
        StopAllCoroutines();
        m_currentHealth = m_maxHealth;
        if (m_useShields) m_currentShieldStrength = m_maxShieldStrength;
    }

    public void TakeDamage(float p_appliedDamage)
    {
        if (!m_isDead)
        {
            DealDamage(p_appliedDamage);
            if (m_isDead)
            {
                m_onDied.Invoke();
            }
        }

    }
    public void HealHealth(float p_appliedHealth)
    {
        if (!m_isDead)
        {
            m_currentHealth += p_appliedHealth;

        }
    }

    public void TakeDamageExplosion(float p_appliedDamage, Vector3 p_explosionPosition, float p_explosionForce, float p_explosionRadius)
    {
        if (!m_isDead)
        {
            DealDamage(p_appliedDamage);
            if (m_isDead)
            {
                m_onDied.Invoke();
            }
        }
    }

    private void DealDamage(float p_appliedDamage)
    {
        if (!m_isDead)
        {
            StopAllCoroutines();

            if (m_useShields && m_currentShieldStrength > 0)
            {
                m_currentShieldStrength -= p_appliedDamage * m_shieldDamageMultiplier;
                if (m_currentShieldStrength < 0)
                {
                    m_currentHealth -= (Mathf.Abs(m_currentShieldStrength * ((1f - m_shieldDamageMultiplier) + 1f)));
                    if (m_currentHealth <= 0)
                    {
                        m_isDead = true;

                    }
                    m_currentShieldStrength = 0;
                }
                if (!m_isDead)
                {
                    m_shieldRegenerationCoroutine = StartCoroutine(RegenShield());
                }
            }

            else
            {
                m_currentHealth -= p_appliedDamage;
                if (m_currentHealth > 0)
                {
                    if (m_useShields)
                    {
                        m_shieldRegenerationCoroutine = StartCoroutine(RegenShield());
                    }
                    else if (m_useHealthRegeneration && m_currentHealth < m_maxHealthRegenerationAmount)
                    {
                        m_healthRegenCorotine = StartCoroutine(RegenHealth());
                    }
                }
                else
                {
                    m_isDead = true;

                }
            }

        }
    }

    IEnumerator RegenShield()
    {
        yield return m_shieldRegenDelayTimer;
        float regenRate = ((m_maxShieldStrength / m_shieldRegenTimeToFull)) / 60f;

        while (m_currentShieldStrength < m_maxShieldStrength)
        {
            m_currentShieldStrength += regenRate;
            yield return null;
        }

        m_currentShieldStrength = m_maxShieldStrength;
        if (m_useHealthRegeneration && m_currentHealth < m_maxHealthRegenerationAmount)
        {
            m_healthRegenCorotine = StartCoroutine(RegenHealth());
        }
        m_shieldRegenerationCoroutine = null;
    }

    IEnumerator RegenHealth()
    {
        yield return m_healthRegenDelayTimer;

        float regenRate = ((m_maxHealth / m_healthRegenTimeToFull) / 60f);

        while (m_currentHealth < m_maxHealthRegenerationAmount)
        {
            m_currentHealth += regenRate;
            yield return null;
        }
        m_currentHealth = m_maxHealthRegenerationAmount;
        m_healthRegenCorotine = null;

    }
}