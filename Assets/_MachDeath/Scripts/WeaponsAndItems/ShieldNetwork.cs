using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror.MachDeath
{
    public class ShieldNetwork : NetworkBehaviour
    {
        public GameObject m_shieldCollider;
        [Command]
        public void CmdActiveShield(bool p_state)
        {
            m_shieldCollider.SetActive(p_state);
            RpcActiveShield(p_state);
        }

        [ClientRpc]
        public void RpcActiveShield(bool p_state)
        {
            m_shieldCollider.SetActive(p_state);
        }
    }
}