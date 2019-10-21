﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemBase : MonoBehaviour
{
    [SerializeField]
    protected WeaponController weaponController;

    [SerializeField]
    protected sObj_ItemStats itemstats;

    [SerializeField]
    protected GameObject hitMarker;

    //Shared parameters and data
    protected AudioSource ItemSound;
    protected Camera fpsCam;
    protected Vector3 rayOrigin;
    protected RaycastHit hit;
    protected bool readyToUse = true;
    protected float? itemCoolDown;

    public enum Item { weapon, utility, misc }
    public Item ItemType;

    protected abstract void GetItemType();
    protected abstract void GetItemData();
    protected abstract void ApplyUseItem();

    //Start is called before the first frame update
    void Start()
    {
        weaponController = GetComponentInParent<WeaponController>();
        fpsCam = GetComponentInParent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UseItem(float? itemCoolDown)
    {
        if (readyToUse)
        {
            this.itemCoolDown = itemCoolDown;
            readyToUse = false;
            Invoke("SetReadyToFire", itemstats._itemCoolDown);

            //rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, .0f));
            ApplyUseItem();
            //weaponSound.Play();
        }
    }


    protected void SetReadyToFire()
    {
        readyToUse = true;
    }
}