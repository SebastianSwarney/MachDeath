using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Gun_Base : MonoBehaviour
{
    
    public BulletType m_bullet;


    private ObjectPooler m_pooler;
    public Transform m_fireSpot;
    private PhotonView m_photonView, m_myPhotonView;

    public bool m_testingNetworking;

    public float m_fireDelay;
    private bool m_canFire = true;


    private void Start()
    {
        m_pooler = ObjectPooler.instance;
        m_myPhotonView = GetComponent<PhotonView>();
    }
    public void InitializeGun(PhotonView p_photonView)
    {
        m_photonView = p_photonView;
    }

    [PunRPC]
    public void RPCFireBullet(Vector3 p_dir)
    {
        Transform newBullet = m_pooler.NewObject(m_bullet.m_bullet, m_fireSpot.position, m_fireSpot.rotation).transform;
        newBullet.LookAt(newBullet.position + p_dir);
        newBullet.GetComponent<Bullet>().SetVariables(newBullet.forward * m_bullet.m_bulletSpeed, m_bullet.m_bulletDamage);
    }

    public void RotateFirePostion(Vector3 p_faceDir, bool p_rotateFireSpot)
    {
        if (p_rotateFireSpot)
        {
            m_fireSpot.LookAt(p_faceDir);
        }
        else
        {
            m_fireSpot.localRotation = Quaternion.identity;
        }
    }

    public void ShootGun()
    {
        if (!CanFire()) return;
        m_canFire = false;
        StartCoroutine(FireRate());
        if (m_testingNetworking)
        {
            m_myPhotonView.RPC("RPCFireBullet", RpcTarget.AllBuffered, m_fireSpot.forward);
        }
        else
        {
            RPCFireBullet(m_fireSpot.forward);
        }
    }

    private IEnumerator FireRate()
    {
        yield return new WaitForSeconds(m_fireDelay);
        m_canFire = true;
    }
    public bool CanFire()
    {
        return m_canFire;
    }
}
