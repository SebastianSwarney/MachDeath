﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponBase : ItemBase
{
    [SerializeField]
    private GameObject spearPrefab;

    [SerializeField]
    private LayerMask targetLayerMask;

    [SerializeField]
    private float sphereCastSize;

    [SerializeField]
    private Image Recticle;

    [SerializeField]
    private float LerpSpeed;

    [SerializeField]
    private AnimationCurve animationCurve;

    private float LerpTimer;

    private Quaternion defaultSpearOrientation;

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
        defaultSpearOrientation = this.transform.localRotation;
        LerpTimer = 0;
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
        spear.GetComponent<Rigidbody>().velocity = (spear.transform.forward) * 100f;
        this.transform.LookAt((CalculateShot()));
    }

    private void TestAssist()
    {
        if (Physics.SphereCast(rayOrigin, sphereCastSize, fpsCam.transform.forward, out hit, Mathf.Infinity, targetLayerMask))
        {
            Debug.Log("I hit " + hit.collider.name);
            this.transform.LookAt(Vector3.Lerp(this.transform.position, (CalculateShotNoMarker()), animationCurve.Evaluate((LerpTimer * LerpSpeed))));
            Debug.DrawLine(transform.position, hit.point, Color.cyan);
            Recticle.color = Color.cyan;
            LerpTimer = 0;
            return;
        }
        else
        {
            LerpTimer += Time.deltaTime;
            Recticle.color = Color.red;
            this.transform.localRotation = Quaternion.Lerp(this.transform.localRotation, defaultSpearOrientation, LerpTimer * LerpSpeed);
            LerpTimer = 0;
        }
    }

    private Vector3 CalculateShot()
    {

        //Sphere cast forward to see if the enemy falls within the cast area on targetLayerMask, if so return the Vector3 of the hit point
        //Using the hitpoint to stear the shot by having the spear container look at the enemy target
        if (Physics.SphereCast(rayOrigin, sphereCastSize, fpsCam.transform.forward, out hit, Mathf.Infinity, targetLayerMask))
        {
            LerpTimer += Time.deltaTime;
            Debug.Log("I hit " + hit.collider.name);
            OnRayCastHit();

            return hit.point;
        }
        else
        {
            //If aim assist is not active, then shoot the spear straight forward
            return (transform.position + this.transform.forward * 100f);
        }
    }

    private Vector3 CalculateShotNoMarker()
    {
        //Sphere cast forward to see if the enemy falls within the cast area on targetLayerMask, if so return the Vector3 of the hit point
        //Using the hitpoint to stear the shot by having the spear container look at the enemy target
        if (Physics.SphereCast(rayOrigin, sphereCastSize, fpsCam.transform.forward, out hit, Mathf.Infinity, targetLayerMask))
        {
            LerpTimer += Time.deltaTime;
            Debug.Log("I hit " + hit.collider.name);
            //OnRayCastHit();

            return hit.point;
        }
        else
        {
            //If aim assist is not active, then shoot the spear straight forward
            return (transform.position + this.transform.forward * 100f);
        }
    }

    //Draw a red dot where the raycast hits
    private void OnRayCastHit()
    {
        Instantiate(hitMarker, hit.point, Quaternion.identity);
    }

    //Draw The Size spherecast
    private void OnDrawGizmos()
    {
        for (int i = 0; i < 100; i++)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position + this.transform.forward * i * 0.5f, sphereCastSize);
        }
    }
}
