using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class WeaponHolder : MonoBehaviour
{

    public Gun_Base m_heldGun;

    [Header("Aim Assist")]
    public LayerMask m_aimAssistMask;
    public float m_aimAssistRadius;
    public float m_aimAssistRange;

    public float m_aimAssistOffset;

    [Header("Debugging")]
    public bool m_debugging;
    public Color m_aimAssistOffsetColor;

    private PhotonView m_photonView;
    private void Awake()
    {
        m_photonView = GetComponent<PhotonView>();
    }
    private void Start()
    {
        m_heldGun.InitializeGun(m_photonView);
        
    }

    public void ShootGun()
    {
        
        PerformAimAssist();
        m_heldGun.ShootGun();
    }

    private void PerformAimAssist()
    {
        RaycastHit hit;
        if (Physics.SphereCast(transform.position + (transform.forward * m_aimAssistOffset), m_aimAssistRadius, transform.forward,out hit,m_aimAssistRange, m_aimAssistMask))
        {
            m_heldGun.RotateFirePostion(hit.point, true);
        }
        else
        {
            m_heldGun.RotateFirePostion(Vector3.zero, false);
        }
    }

    private void OnDrawGizmos()
    {
        if (!m_debugging) return;

        Gizmos.color = m_aimAssistOffsetColor;
        Gizmos.DrawWireSphere(transform.position + (transform.forward * m_aimAssistOffset), m_aimAssistRadius);
        Gizmos.DrawLine(transform.position + (transform.forward * m_aimAssistOffset), transform.position + (transform.forward * m_aimAssistRange));
        Gizmos.DrawWireSphere(transform.position + (transform.forward * m_aimAssistRange), m_aimAssistRadius);
    }
}
