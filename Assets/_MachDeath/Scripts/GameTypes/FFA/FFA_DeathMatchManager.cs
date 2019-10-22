using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror.MachDeath{

    public class FFA_DeathMatchManager : GameTypeManager
    {
        [System.Serializable]
        public class PlayerStats
        {
            public string m_playerName;
            public PlayerProperties m_player;
            public int m_killCount;
            
        }

        public List<PlayerStats> m_playerStats;
        public int m_killsToWin;
        public float m_matchTime;



        [Server]
        private void OnEnable()
        {
            GameTypeManager.Instance = this;
        }

        [Server]

        private void OnDisable()
        {
            GameTypeManager.Instance = null;
        }
        [Server]
        public override void PlayerDied(Health p_playerHealth, PlayerProperties p_projectileOwner)
        {
            foreach(PlayerStats newStat in m_playerStats)
            {
                if (newStat.m_player == p_projectileOwner)
                {
                    newStat.m_killCount += 1;
                    if (newStat.m_killCount >= m_killsToWin)
                    {
                        
                    }
                }
            }
        }

        private void GameWin(PlayerProperties p_player)
        {

        }

        
    }

}