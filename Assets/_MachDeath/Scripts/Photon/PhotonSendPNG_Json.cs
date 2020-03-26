using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using System.Text;
using System;
public class PhotonSendPNG_Json : MonoBehaviour
{
    private PhotonView m_photonView;

    public UnityEngine.UI.InputField m_inputField;
    public UnityEngine.UI.InputField m_textureName;
    public UnityEngine.UI.Text m_textureText;

    private void Start()
    {
        m_photonView = GetComponent<PhotonView>();
    }
    public void ChangeTexture()
    {

        if (m_photonView.IsMine)
        {
            TextureHolder newTexture = new TextureHolder();
            newTexture.textureType = StoreTextureAsBytes();
            newTexture.p_textureName = m_textureName.text;
            if (newTexture.textureType == null)
            {
                m_textureText.text = "Path did not exist";
                return;
            }
            string jsonData = JsonUtility.ToJson(newTexture);
            m_photonView.RPC("LoadTexture", RpcTarget.AllBuffered, jsonData);
            //m_photonView.RPC("LoadTexture", RpcTarget.AllBuffered);
        }
    }

    private string StoreTextureAsBytes()
    {
        if (File.Exists(m_inputField.text))
        {
            byte[] buffer = File.ReadAllBytes(m_inputField.text);
            return BitConverter.ToString(buffer);
        }
        else
        {
            return null;
        }
    }

    [PunRPC]

    private void LoadTexture(string p_json)
    {
        TextureHolder textureHold = JsonUtility.FromJson<TextureHolder>(p_json);
        m_textureText.text = "Loading Texture: " + textureHold.p_textureName;
        //print("bytes: " + textureHold.textureType);

        Texture2D newTex = new Texture2D(2, 2);
        newTex.LoadImage(Encoding.UTF8.GetBytes(textureHold.textureType));
        Billboard.Instance.m_renderer.material.SetTexture("_MainTex", newTex);


    }

}
[System.Serializable]
public class TextureHolder
{

   [SerializeField] public string textureType;

   [SerializeField] public string p_textureName;
}
