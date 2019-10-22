using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachDeathSpawnPoint : MonoBehaviour
{

    public PlayerProperties.PlayerTeam m_spawnPointTeam;
    public float m_playerDetectionRadius;
    public LayerMask m_playerMask;
    [Header("Debugging")]
    public bool m_isDebugging = true;
    public Color m_debuggingColor;

    public bool PlayerCloseToSpawn()
    {
        return !(Physics.OverlapSphere(transform.position, m_playerDetectionRadius, m_playerMask).Length < 0);
    }

    private void OnDrawGizmos()
    {
        if (!m_isDebugging)
        {
            return;
        }
        Gizmos.color = m_debuggingColor;
        Gizmos.DrawWireSphere(transform.position, m_playerDetectionRadius);
    }
}
