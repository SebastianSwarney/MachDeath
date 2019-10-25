using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror.MachDeath
{
    public class ObjectPooler_Network : MonoBehaviour
    {        
        #region Variables
        private Queue<GameObject> m_objectPool = new Queue<GameObject>();
        public GameObject m_pooledObject;

        public int m_growthRate = 5;
        public int m_initalPoolSize = 5;


        public System.Guid assetId{ get; set;}
        public delegate GameObject SpawnDelegate(Vector3 p_position, System.Guid p_assetId);
        public delegate void UnSpawnDelegate(GameObject p_pooledObject);
        #endregion

        

        #region Object pooler

        public void InitializePooler(GameObject p_pooledObject)
        {
            m_pooledObject = p_pooledObject;
            assetId = m_pooledObject.GetComponent<NetworkIdentity>().assetId;
            InitialGrowth();
            ClientScene.RegisterSpawnHandler(assetId, SpawnObject, ReturnToPool);
        }

        public GameObject NewObject(Vector3 p_pos)
        {
            if (m_objectPool.Count == 0)
            {
                IncreasePool();
            }
            GameObject spawnedObject = m_objectPool.Dequeue();

            spawnedObject.transform.position = p_pos;
            return spawnedObject;
        }

        public GameObject SpawnObject(Vector3 position, System.Guid assetId)
        {
            return NewObject(position);
        }

        ///<summary>
        ///When the pool is equal to zero, increase the pool
        ///called in the NewObject function
        ///<summary>
        private void IncreasePool()
        {
            for (int i = 0; i < m_growthRate; i++)
            {
                GameObject newObj = Instantiate(m_pooledObject);
                newObj.transform.parent = transform;
                newObj.SetActive(false);
                m_objectPool.Enqueue(newObj);
            }
        }

        ///<summary>
        ///Returns the object to it's designated pool
        ///Called from the object
        ///<summary>
        public void ReturnToPool(GameObject pooledObject)
        {
            m_objectPool.Enqueue(pooledObject);
            pooledObject.SetActive(false);
        }




        ///<summary>
        ///This function is only called at start
        ///creates all the pools, and puts then under the right transform
        ///<summary>
        void InitialGrowth()
        {
            for (int i = 0; i < m_initalPoolSize; i++)
            {
                GameObject newObj = Instantiate(m_pooledObject);
                newObj.transform.parent = transform;
                m_objectPool.Enqueue(newObj);
                newObj.SetActive(false);
            }
        }
        #endregion
    }
}