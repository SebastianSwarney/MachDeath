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
        public MapTypeSetup m_mapSetup;
        private Coroutine m_matchTimer;

        private List<PlayerStats> m_possibleWinners = new List<PlayerStats>();

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
                        GameWin(newStat.m_player);
                        StopCoroutine(m_matchTimer);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// The Match Timer. Tallies the kills when the timer is out
        /// </summary>
        private IEnumerator MatchTimer()
        {
            yield return new WaitForSeconds(m_matchTime);
            CheckWinners();
        }

        /// <summary>
        /// Called when a player wins.
        /// </summary>
        /// <param name="p_player"></param>
        private void GameWin(PlayerProperties p_player)
        {
            if (m_mapSetup.m_victoryScenePositions.Count >0)
            {
                p_player.transform.position = m_mapSetup.m_victoryScenePositions[0].position;
            }
            m_gameStateEvents.m_gameOverState.Invoke();
        }

        /// <summary>
        /// Called from the timer. Tallies the kills and names the player with the highest the winner. Or, if more than one player has the same top score, it ties it.
        /// </summary>
        private void CheckWinners()
        {
            int currentWinnerAmount = 0;
            bool tieGame = false;

            //Tallies the kill counts
            foreach (PlayerStats stat in m_playerStats)
            {
                if (stat.m_killCount > currentWinnerAmount)
                {
                    currentWinnerAmount = stat.m_killCount;
                    m_possibleWinners.Clear();
                    m_possibleWinners.Add(stat);
                    tieGame = false;
                }else if (stat.m_killCount == currentWinnerAmount)
                {
                    tieGame = true;
                    m_possibleWinners.Add(stat);
                }
            }

            //Check if the game is won, or if its tied
            if (tieGame)
            {
                m_gameStateEvents.m_gameTied.Invoke();
                print("Game Tied");

            }
            else
            {
                GameWin(m_possibleWinners[0].m_player);
                print("Game Win");
            }
        }

        
    }

}