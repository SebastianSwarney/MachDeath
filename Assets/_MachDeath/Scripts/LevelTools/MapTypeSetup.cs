using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror.MachDeath {
    [RequireComponent(typeof(MachDeathSpawningManager))]
    public class MapTypeSetup :MonoBehaviour
    {
        public List<Transform> m_victoryScenePositions;
        
        public MachDeathSpawningManager m_spawnManager;
        public Mirror.NetworkManager m_networkManager;

        private void Start()
        {
            if (m_spawnManager.m_spawnPoints.Count < m_networkManager.maxConnections)
            {
                throw new System.Exception("Not enough spawn points. You need atleast " + m_networkManager.maxConnections + " spawn points");
            }
        }
    }
}