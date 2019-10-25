using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror.MachDeath{
    public class NetworkPlayer : NetworkBehaviour
    {
        private PlayerInput m_input;
        private PlayerMovementController m_movementController;
        public Camera m_camera;
        public KeyCode m_pauseKey;
        private bool m_mouseLocked = true;


        private void Awake()
        {
            m_input = GetComponent<PlayerInput>();
            m_movementController = GetComponent<PlayerMovementController>();
            

        }

        private void Start()
        {
            if (!isLocalPlayer)
            {
                m_camera.enabled = false;
            }
        }

        private void Update()
        {
            if (!isLocalPlayer) return;
            if (Input.GetKeyDown(m_pauseKey))
            {
                Cursor.lockState = (m_mouseLocked) ? CursorLockMode.None : CursorLockMode.Locked;
                m_mouseLocked = !m_mouseLocked;
            }
            m_input.GetInput();
            m_movementController.PerformController();

        }
    }
}
