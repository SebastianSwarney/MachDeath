using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror.MachDeath
{
    [System.Serializable]
    public class GameStateEvent : UnityEngine.Events.UnityEvent { }

    public abstract class GameTypeManager : NetworkBehaviour
    {
        [System.Serializable]
        public struct GameStateEvents
        {
            public GameStateEvent m_gameOverState;
            public GameStateEvent m_gameTied;
        }

        public List<GameObject> m_winningPlayers;

        public GameStateEvents m_gameStateEvents;
        public static GameTypeManager Instance { get; set; }
        public abstract void PlayerDied(Health p_playerHealth, PlayerProperties p_projectileOwner);

    }
}