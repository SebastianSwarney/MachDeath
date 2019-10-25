using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGraphicsController : MonoBehaviour
{
    private Animator m_animator;

    private PlayerMovementController m_movementController;

    private CharacterController m_charController;

    private void Start()
    {
        m_animator = GetComponentInChildren<Animator>();
        m_movementController = GetComponent<PlayerMovementController>();
        m_charController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Vector3 reletiveVelocity = transform.InverseTransformDirection(m_movementController.m_velocity);

        m_animator.SetFloat("Forward", reletiveVelocity.z);
        m_animator.SetFloat("Strafe", reletiveVelocity.x);
    }
}
