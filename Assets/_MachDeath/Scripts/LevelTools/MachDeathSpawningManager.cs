using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachDeathSpawningManager : MonoBehaviour
{
    public static MachDeathSpawningManager Instance { get; private set; }
    public List<MachDeathSpawnPoint> m_spawnPoints;
    private List<MachDeathSpawnPoint> m_randomizedSpawns = new List<MachDeathSpawnPoint>();
    
    private void Awake()
    {
        Instance = this;

        for (int i = 0; i < transform.childCount; i++)
        {
            m_spawnPoints.Add(transform.GetChild(i).GetComponent<MachDeathSpawnPoint>());
        }

        foreach(MachDeathSpawnPoint spawn in m_spawnPoints)
        {
            m_randomizedSpawns.Add(spawn);
        }
    }


    /// <summary>
    ///Returns a spawn point in Free For All
    ///If there is a player in the radius of the spawn point, avoids that spawn if possible
    /// </summary>

    public Transform NewSpawnPointFFA()
    {
        RandomizeSpawnList();
        foreach(MachDeathSpawnPoint m_rand in m_randomizedSpawns)
        {
            if (!m_rand.PlayerCloseToSpawn())
            {
                return m_rand.transform;
            }
            
        }
        return m_randomizedSpawns[Random.Range(0, m_randomizedSpawns.Count)].transform;
    }


    /// <summary>
    /// Randomizes the spawning list, so that the possibility of spawning in the same spot over and over is reduced
    /// </summary>
    private void RandomizeSpawnList()
    {
        for (int i = 0; i < m_spawnPoints.Count; i++)
        {
            MachDeathSpawnPoint tempTrans = m_randomizedSpawns[i];
            int randomIndex = Random.Range(0, m_spawnPoints.Count);
            m_randomizedSpawns[i] = m_randomizedSpawns[randomIndex];
            m_randomizedSpawns[randomIndex] = tempTrans;
        }
    }
}
