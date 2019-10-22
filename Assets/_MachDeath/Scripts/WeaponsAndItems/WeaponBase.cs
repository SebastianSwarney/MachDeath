using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : ItemBase
{
    [SerializeField]
    private GameObject spearPrefab;

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
        ShootGun();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void ShootGun()
    {
        GameObject spear = Instantiate(spearPrefab);
        spear.transform.position = this.transform.position;
        spear.transform.rotation = this.transform.rotation;
        spear.GetComponent<Rigidbody>().velocity = this.transform.parent.forward * 20f;
    }
}
