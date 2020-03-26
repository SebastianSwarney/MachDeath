using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class PhotonSpawnObject : MonoBehaviour
{
    private PhotonView m_photonView;

    public GameObject m_prefab;
    private ObjectPooler m_pooler;

    public UnityEngine.UI.Text m_loadingText;

    [Header("Pointer")]
    public Transform m_firespot;
    public LayerMask m_mask;

    private void Start()
    {
        m_photonView = GetComponent<PhotonView>();
        m_pooler = ObjectPooler.instance;
    }

    public void SpawnObjector()
    {
        if (!m_photonView.IsMine) return;

        RaycastHit hit;
        if (Physics.Raycast(m_firespot.position,m_firespot.forward,out hit, 200f, m_mask)) {
            DataHolder newData = new DataHolder();
            newData.text = "Spawning: " + m_prefab.gameObject.name;
            newData.posX = hit.point.x;
            newData.posY = hit.point.y;
            newData.posZ = hit.point.z;

            m_photonView.RPC("RPC_SpawnOnServer", RpcTarget.AllBuffered, JsonUtility.ToJson(newData));
        }
    }

    [PunRPC]
    private void RPC_SpawnOnServer(string p_jsonData)
    {
        print("Create");

        DataHolder newData = JsonUtility.FromJson<DataHolder>(p_jsonData);
        m_pooler.NewObject(m_prefab, new Vector3(newData.posX, newData.posY, newData.posZ), Quaternion.identity);
        m_loadingText.text = newData.text;
    }
}
[System.Serializable]
public class DataHolder
{
    public string text;
    public float posX, posY, posZ;
}
