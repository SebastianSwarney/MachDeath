using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : ItemBase
{
    [SerializeField]
    private GameObject spearPrefab;

    [SerializeField]
    private LayerMask targetLayerMask;

    // Start is called before the first frame update
    void Start()
    {
        GetItemType();
        GetItemData();
    }
    protected override void GetItemType()
    {
        ItemType = Item.weapon;
    }

    protected override void GetItemData()
    {
        //Get reference to fps Camera and WeaponController//Should be item controller
        base.GetData();
    }

    protected override void ApplyUseItem()
    {
        Debug.Log("Using Weapon");
        ShootGun();
    }

    // Update is called once per frame
    void Update()
    {
        TestAssist();
    }
    private void ShootGun()
    {
        //CalculateShot();
        //this.transform.LookAt((CalculateShot().point) + new Vector3(0,-90,0));

        GameObject spear = Instantiate(spearPrefab);
        spear.transform.position = this.transform.position;
        spear.transform.rotation = this.transform.rotation;
        spear.GetComponent<Rigidbody>().velocity = this.transform.parent.forward * 100f;
        spear.transform.LookAt((CalculateShot().point) + new Vector3(0, -90, 0) - spear.transform.position);
    }

    private void TestAssist()
    {
        if (Physics.SphereCast(rayOrigin, 3.5f, fpsCam.transform.forward, out hit, Mathf.Infinity, ~targetLayerMask))
        {
            Debug.Log("I hit " + hit.collider.name);
            this.transform.LookAt((CalculateShotNoMarker().point) + new Vector3(0, -90, 0));
            return;
        }
        else
        {
            this.transform.rotation = this.transform.rotation;
        }
    }

    private RaycastHit CalculateShot()
    {
        if (Physics.SphereCast(rayOrigin, 3.5f, fpsCam.transform.forward, out hit, Mathf.Infinity, ~targetLayerMask))
        {
            Debug.Log("I hit " + hit.collider.name);
            OnRayCastHit();

            return hit;
        }
        else
        {
            return hit;
        }

        //if (Physics.Raycast(rayOrigin, 0.1f, fpsCam.transform.forward, out hit, Mathf.Infinity))
        //{
        //    Debug.Log("I hit " + hit.collider.name);
        //    OnRayCastHit();

        //    return hit;
        //}
        //else
        //{
        //    return hit;
        //}
    }

    private RaycastHit CalculateShotNoMarker()
    {
        if (Physics.SphereCast(rayOrigin, 3.5f, fpsCam.transform.forward, out hit, Mathf.Infinity))
        {
            //Debug.Log("I hit " + hit.collider.name);
            //OnRayCastHit();

            return hit;
        }
        else
        {
            return hit;
        }

        //if (Physics.Raycast(rayOrigin, 0.1f, fpsCam.transform.forward, out hit, Mathf.Infinity))
        //{
        //    Debug.Log("I hit " + hit.collider.name);
        //    OnRayCastHit();

        //    return hit;
        //}
        //else
        //{
        //    return hit;
        //}
    }

    private void OnRayCastHit()
    {
        Instantiate(hitMarker, hit.point, Quaternion.identity);
    }
}
