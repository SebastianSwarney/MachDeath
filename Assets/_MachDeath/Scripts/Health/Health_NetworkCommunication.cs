using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror.MachDeath
{
    public class Health_NetworkCommunication : NetworkBehaviour
    {
        public float m_deathTimer;
        public HealthEvent m_healthEvent;
        private Rigidbody m_rb;
        private MachDeathSpawningManager m_spawnManager;
        public GameObject m_visualState;
        private Coroutine m_diedCoroutine;
        public PlayerInput m_input;
        public PlayerMovementController m_movementCont;

        private GameTypeManager m_currentGameType;
        private PlayerProperties m_playerProperties;
        private void Start()
        {
            m_currentGameType = GameTypeManager.Instance;
            m_rb = GetComponent<Rigidbody>();
            m_spawnManager = MachDeathSpawningManager.Instance;
            m_playerProperties = GetComponent<PlayerProperties>();
        }

        
        public void OnDied()
        {
            if (m_diedCoroutine == null)
            {
                DisablePlayer(null);
                m_diedCoroutine = StartCoroutine(DiedCoroutine());
            }

        }

        
        private void DisablePlayer(PlayerProperties p_killer)
        {
            m_visualState.SetActive(false);

            m_input.enabled = false;
            m_movementCont.enabled = false;
            if (m_currentGameType != null)
            {
                m_currentGameType.PlayerDied(m_playerProperties, p_killer);
            }
            m_diedCoroutine = StartCoroutine(DiedCoroutine());

        }

        
        public void OnKilled(PlayerProperties p_killer)
        {
            if (m_diedCoroutine == null)
            {
                DisablePlayer(p_killer);
            }

        }

        /// <summary>
        /// The respawn coroutine. Waits until the repsawn time is up, and picks a new spawn position
        /// </summary>
        /// <returns></returns>
        private IEnumerator DiedCoroutine()
        {
            yield return new WaitForSeconds(m_deathTimer);


            

            m_diedCoroutine = null;
            if (isLocalPlayer)
            {
                Transform newSpawn = m_spawnManager.NewSpawnPointFFA();
                transform.position = newSpawn.position;
                transform.eulerAngles = new Vector3(0f, newSpawn.transform.eulerAngles.y, 0f);
                m_healthEvent.Invoke();
            }
            m_visualState.SetActive(true);

            yield return new WaitForFixedUpdate();
            m_input.enabled = true;
            m_movementCont.enabled = true;
        }

        

    }
}