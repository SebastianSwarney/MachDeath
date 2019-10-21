using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityBase : ItemBase
{
    void Start()
    {
        GetItemType();
    }
    protected override void GetItemType()
    {
        ItemType = Item.utility;
    }
    protected override void GetItemData()
    {
        
    }

    protected override void ApplyUseItem()
    {
        Debug.Log("Using Shield");
    }
    // Start is called before the first frame update
  

    // Update is called once per frame
    void Update()
    {
        
    }
}
