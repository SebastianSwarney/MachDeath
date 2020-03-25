using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bullet Type", menuName = "Bullets", order = 0)]
public class BulletType : ScriptableObject
{
    public GameObject m_bullet;
    public float m_bulletSpeed;
    public float m_bulletDamage;
}
