using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiscBase : ItemBase
{
    // Start is called before the first frame update
    void Start()
    {
        GetItemType();
    }
    protected override void GetItemType()
    {
        ItemType = Item.misc;
    }

    protected override void GetItemData()
    {

    }

    protected override void ApplyUseItem()
    {
  
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
