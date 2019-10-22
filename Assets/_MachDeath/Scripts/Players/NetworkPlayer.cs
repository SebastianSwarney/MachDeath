using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror.MachDeath{
    public class NetworkPlayer : NetworkBehaviour
    {
        private PlayerInput m_input;
        private PlayerMovementController m_movementController;
        private SinglePlayer m_singlePlayer;

        private void Awake()
        {
            m_input = GetComponent<PlayerInput>();
            m_movementController = GetComponent<PlayerMovementController>();
            m_singlePlayer = GetComponent<SinglePlayer>();

        }

        private void Start()
        {
            if (m_singlePlayer.enabled)
            {
                //m_singlePlayer.enabled = false;
            }
        }

        private void Update()
        {
            m_input.GetInput();
            m_movementController.PerformController();
        }
    }
}
