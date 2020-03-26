using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public class PhotonSendPNG_Binary : MonoBehaviour
{
    /*private PhotonView m_photonView;

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

            BinaryFormatter binaryF = new BinaryFormatter();
            byte[] binaryData;
            using (MemoryStream memorySteam = new MemoryStream())
            {
                binaryF.Serialize(memorySteam, newTexture);
                binaryData = memorySteam.ToArray();
            }


            m_photonView.RPC("LoadTexture", RpcTarget.AllBuffered, binaryData);
        }
    }

    private byte[] StoreTextureAsBytes()
    {
        if (File.Exists(m_inputField.text))
        {
            return File.ReadAllBytes(m_inputField.text);
        }
        else
        {
            return null;
        }
    }

    [PunRPC]

    private void LoadTexture(byte[] p_json)
    {

        TextureHolder textureHold = null;
        using (MemoryStream memorystream = new MemoryStream())
        {
            BinaryFormatter bf = new BinaryFormatter();
            memorystream.Write(p_json, 0, p_json.Length);
            memorystream.Seek(0, SeekOrigin.Begin);
            textureHold = (TextureHolder)bf.Deserialize(memorystream);
        }

        //m_textureText.text = "Loading Texture: " + textureHold.p_textureName;
        print("Oof");

        Texture2D newTex = new Texture2D(2, 2);
        newTex.LoadImage(textureHold.textureType);
        Billboard.Instance.m_renderer.material.SetTexture("_MainTex", newTex);


    }
    */
}



