using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinglePlayer : MonoBehaviour
{
    private PlayerInput m_input;
    private PlayerMovementController m_movementController;

    private void Awake()
    {
        m_input = GetComponent<PlayerInput>();
        m_movementController = GetComponent<PlayerMovementController>();
    }
    
    private void Update()
    {
        m_input.GetInput();
        m_movementController.PerformController();
    }
}
