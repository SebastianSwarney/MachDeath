using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : ItemBase
{
    // Start is called before the first frame update
    void Start()
    {
        GetItemType();
    }
    protected override void GetItemType()
    {
        ItemType = Item.weapon;
    }

    protected override void GetItemData()
    {

    }

    protected override void ApplyUseItem()
    {
        Debug.Log("Using Weapon");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
