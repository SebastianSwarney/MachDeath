using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror.MachDeath {
    [RequireComponent(typeof(MachDeathSpawningManager))]
    public class GameTypeSetup :MonoBehaviour
    {
        public List<Transform> m_victoryScenePositions;
        private MachDeathSpawningManager m_spawnPoints;
        public Mirror.NetworkManager m_networkManager;

        private void Awake()
        {

        }
    }
}