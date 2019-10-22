using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class sObj_ItemStats : ScriptableObject
{

    [SerializeField]
    private float itemCoolDown;
    public float _itemCoolDown => itemCoolDown;

    [SerializeField]
    private GameObject hitMarker;
    public GameObject _hitMarker => hitMarker;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
