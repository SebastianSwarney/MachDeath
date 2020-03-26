using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public static Billboard Instance;
    public MeshRenderer m_renderer;

    private void Awake()
    {
        Instance = this;
    }
}
