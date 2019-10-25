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

    public Transform m_leftShoulder;

    public Transform m_lookTarget;

    private ReletiveTransform m_trans;

    public GraphicsEvent m_downEvent = new GraphicsEvent();
    public GraphicsEvent m_upEvent = new GraphicsEvent();

    public GraphicsEvent m_spearDownEvent = new GraphicsEvent();
    public GraphicsEvent m_spearUpEvent = new GraphicsEvent();

    private void Start()
    {
        m_animator = GetComponent<Animator>();
        m_movementController = GetComponentInParent<PlayerMovementController>();
        m_charController = GetComponentInParent<CharacterController>();

        m_trans = GetComponentInChildren<ReletiveTransform>();
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

        if (Input.GetMouseButtonDown(1))
        {
            m_animator.SetBool("Holding Attack", true);
            m_spearDownEvent.Invoke();
        }

        if (Input.GetMouseButtonUp(1))
        {
            m_animator.SetBool("Holding Attack", false);
            m_spearUpEvent.Invoke();
        }

        Vector3 reletiveVelocity = transform.InverseTransformDirection(m_movementController.m_velocity);

        m_animator.SetFloat("Forward", reletiveVelocity.z);
        m_animator.SetFloat("Strafe", reletiveVelocity.x);
        
    }

    public void OnAnimatorIK(int layerIndex)
    {
        Quaternion shoulder = m_animator.GetBoneTransform(HumanBodyBones.LeftShoulder).localRotation;



        Quaternion targetRot = Quaternion.AngleAxis(m_lookTarget.eulerAngles.x, Vector3.right);

        //m_animator.SetBoneLocalRotation(HumanBodyBones.LeftShoulder, shoulder * targetRot);

        m_trans.m_localRotation *= targetRot;
    }

}
