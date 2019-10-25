using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReletiveTransform : MonoBehaviour
{
    [HideInInspector]
    public Quaternion m_localRotation;

    private Quaternion startingLocalRotation;

    // Use this for initialization
    void Start()
    {
        startingLocalRotation = transform.localRotation;
    }

    void LateUpdate()
    {
        transform.localRotation = startingLocalRotation * m_localRotation;
    }
}
