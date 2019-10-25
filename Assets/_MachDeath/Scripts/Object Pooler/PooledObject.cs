using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror.MachDeath
{
    public class PooledObject : NetworkBehaviour
    {
        public ObjectPooler_Network m_pooler;
        public void DestroyObject()
        {
            m_pooler.ReturnToPool(this.gameObject);
            CmdDespawnItem(this.gameObject);
        }
        [Command]
        public void CmdDespawnItem(GameObject p_object)
        {
            NetworkServer.UnSpawn(p_object);
        }

    }
}