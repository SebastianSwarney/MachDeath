using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Events;

[System.Serializable]
public class GraphicsEvent : UnityEvent { }

public class PlayerGraphicsController : MonoBehaviour
{
    private Animator m_animator;

    private PlayerMovementController m_movementController;

    private CharacterController m_charController;

    public Transform m_lookTarget;

    public GraphicsEvent m_downEvent = new GraphicsEvent();
    public GraphicsEvent m_upEvent = new GraphicsEvent();

    private void Start()
    {
        m_animator = GetComponent<Animator>();
        m_movementController = GetComponentInParent<PlayerMovementController>();
        m_charController = GetComponentInParent<CharacterController>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            m_animator.SetBool("Holding Block", true);
            m_downEvent.Invoke();
        }

        if (Input.GetMouseButtonUp(0))
        {
            m_animator.SetBool("Holding Block", false);
            m_upEvent.Invoke();
        }

        Vector3 reletiveVelocity = transform.InverseTransformDirection(m_movementController.m_velocity);

        m_animator.SetFloat("Forward", reletiveVelocity.z);
        m_animator.SetFloat("Strafe", reletiveVelocity.x);
        
    }

    public void OnAnimatorIK(int layerIndex)
    {
        Quaternion q = Quaternion.AngleAxis(m_lookTarget.eulerAngles.x, Vector3.right);

        m_animator.SetBoneLocalRotation(HumanBodyBones.LeftShoulder, q);
        m_animator.SetBoneLocalRotation(HumanBodyBones.RightShoulder, q);
        m_animator.SetBoneLocalRotation(HumanBodyBones.Neck, q);
    }

}
