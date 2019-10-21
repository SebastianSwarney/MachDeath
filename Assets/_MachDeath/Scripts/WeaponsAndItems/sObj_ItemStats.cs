using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class sObj_ItemStats : ScriptableObject
{

    [SerializeField]
    private float itemCoolDown;
    public float _itemCoolDown => itemCoolDown;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
