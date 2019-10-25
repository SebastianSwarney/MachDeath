using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror.MachDeath
{
    public class PooledObject : NetworkBehaviour
    {
        public ObjectPooler_Network m_pooler;
        public bool m_networkObject = true;
        private ObjectPooler m_clientPooler;
        public void DestroyObject()
        {
            if (m_networkObject)
            {
                m_pooler.ReturnToPool(this.gameObject);
                CmdDespawnItem(this.gameObject);
            }
            else
            {
                if (m_clientPooler == null)
                {
                    m_clientPooler = ObjectPooler.instance;
                }
                m_clientPooler.ReturnToPool(this.gameObject);
            }
            
        }
        [Command]
        public void CmdDespawnItem(GameObject p_object)
        {
            NetworkServer.UnSpawn(p_object);
        }

    }
}