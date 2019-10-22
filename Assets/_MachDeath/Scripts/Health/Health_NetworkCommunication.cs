using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror.MachDeath
{
    public class Health_NetworkCommunication : MonoBehaviour
    {
        public float m_deathTimer;
        public HealthEvent m_healthEvent;
        private Rigidbody m_rb;
        private MachDeathSpawningManager m_spawnManager;
        public GameObject m_visualState;
        private Coroutine m_diedCoroutine;
        public PlayerInput m_input;
        public PlayerMovementController m_movementCont;
        private void Start()
        {
            m_rb = GetComponent<Rigidbody>();
            m_spawnManager = MachDeathSpawningManager.Instance;

        }
        public void OnDied()
        {
            if (m_diedCoroutine == null)
            {
                m_visualState.SetActive(false);
                m_diedCoroutine = StartCoroutine(DiedCoroutine());
                m_input.enabled = false;
                m_movementCont.enabled = false;
            }

        }

        private IEnumerator DiedCoroutine()
        {
            yield return new WaitForSeconds(m_deathTimer);
            Transform newSpawn = m_spawnManager.NewSpawnPointFFA();
            transform.position = newSpawn.position;
            transform.eulerAngles = new Vector3(0f, newSpawn.transform.eulerAngles.y, 0f);
            m_healthEvent.Invoke();
            m_visualState.SetActive(true);
            m_diedCoroutine = null;
            yield return new WaitForFixedUpdate();
            m_input.enabled = true;
            m_movementCont.enabled = true;
        }
    }
}