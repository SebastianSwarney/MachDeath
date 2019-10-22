using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityBase : ItemBase
{
    void Start()
    {
        GetItemType();
        GetItemData();
    }
    protected override void GetItemType()
    {
        ItemType = Item.utility;
    }
    protected override void GetItemData()
    {
        //Get reference to fps Camera and WeaponController//Should be item controller
        base.GetData();
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
