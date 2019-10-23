using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror.MachDeath
{
    public class MachDeathNetworkManager : NetworkManager
    {
        public static MachDeathNetworkManager Instance { get; private set; }
        public GameTypeManager m_gametype;
        public MachDeathSpawningManager m_spawnManager;

        public override void Awake()
        {
            base.Awake();
            Instance = this;
        }

        public override void OnStartServer()
        {
            base.OnStartServer();
            NetworkServer.RegisterHandler<CreateCharacterMessage>(OnCreateCharacter);
        }
        public override void OnClientConnect(NetworkConnection conn)
        {
            base.OnClientConnect(conn);
            CreateCharacterMessage characterMessage = new CreateCharacterMessage();
            conn.Send(characterMessage);
        }

        void OnCreateCharacter(NetworkConnection conn, CreateCharacterMessage message)
        {
            GameObject newPlayer = Instantiate(playerPrefab);

            newPlayer.transform.position = m_spawnManager.NewSpawnPointFFA().position;

            PlayerProperties playerProp = newPlayer.GetComponent<PlayerProperties>();
            playerProp.m_playerName = "Ya Boi";
            playerProp.m_playerName = message.m_playerName;
            m_gametype.AssignPlayer(playerProp);
            NetworkServer.AddPlayerForConnection(conn, newPlayer);

        }

    }

    public class CreateCharacterMessage : MessageBase
    {
        public string m_playerName;
        
    }
}