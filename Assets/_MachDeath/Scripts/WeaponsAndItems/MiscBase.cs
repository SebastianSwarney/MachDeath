using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiscBase : ItemBase
{
    // Start is called before the first frame update
    void Start()
    {
        GetItemType();
        GetItemData();
    }
    protected override void GetItemType()
    {
        ItemType = Item.misc;
    }

    protected override void GetItemData()
    {
        //Get reference to fps Camera and WeaponController//Should be item controller
        base.GetData();
    }

    protected override void ApplyUseItem()
    {
        Debug.Log("Using Misc");
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
