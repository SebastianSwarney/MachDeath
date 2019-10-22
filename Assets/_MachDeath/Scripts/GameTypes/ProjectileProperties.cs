using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileProperties : MonoBehaviour
{
    public PlayerProperties m_spearOwner;


    public void AssignSpear(PlayerProperties m_owner)
    {
        m_spearOwner = m_owner;
    }
}
