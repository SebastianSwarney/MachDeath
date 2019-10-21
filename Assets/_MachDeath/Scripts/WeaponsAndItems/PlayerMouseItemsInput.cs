using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMouseItemsInput : MonoBehaviour
{
    [SerializeField]
    private WeaponController weaponController;

    //start the index on 0
    private int currentLeftUsableIndex = 0;
    private int currentRightUsableIndex = 0;
 
    // Start is called before the first frame update
    void Start()
    {
        weaponController = GetComponent<WeaponController>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckMouseScroll();
        ItemUse();
    }

    void CheckMouseScroll()
    {
        var Items = weaponController.usableItemsLeftClick;
        if (Input.GetAxis("Mouse ScrollWheel") > .0f)
        {
            Items[currentLeftUsableIndex].gameObject.SetActive(false);
            currentLeftUsableIndex = (currentLeftUsableIndex + 1) % (Items.Count);
            Items[currentLeftUsableIndex].gameObject.SetActive(true);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < .0f)
        {
            Items[currentLeftUsableIndex].gameObject.SetActive(false);

            if (currentLeftUsableIndex == 0)
            {
                currentLeftUsableIndex = Items.Count - 1;
            }
            else
            {
                currentLeftUsableIndex = (currentLeftUsableIndex - 1) % (Items.Count);
            }

            Items[currentLeftUsableIndex].gameObject.SetActive(true);
        }
    }

    void ItemUse()
    {
        Debug.Log("current Index is: " + currentLeftUsableIndex);
        var Items = weaponController.usableItemsLeftClick;

        if (Items[currentLeftUsableIndex])
        {
            if (Input.GetButtonDown("Fire1"))
            {
                var isWeapon = Items[currentLeftUsableIndex].GetComponent<WeaponBase>();
                if (isWeapon)
                {
                    isWeapon.UseItem(0f);
                }
                else
                {
                    Items[currentLeftUsableIndex].GetComponent<MiscBase>().UseItem(0f);
                }
            }
            else
            {
                //If there was an automatic weapon such as a gun
                //Uncomment the below code
                //firingSince = .0f;
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1"))
            {
                //weapons[currentWeaponIndex].Shoot(null);
            }
        }

        var Items2 = weaponController.usableItemsRightClick;


        //For shield can not really use this index since the mouse scroll wheel is always set to the usable items 
        //such as weapons and 

        if (Items2[currentRightUsableIndex])
        {
            if (Input.GetButtonDown("Fire2"))
            {
                Items2[currentRightUsableIndex].GetComponent<UtilityBase>().UseItem(0f);
            }
            else
            {
                //firingSince = .0f;
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire2"))
            {
                //weapons[currentWeaponIndex].Shoot(null);
            }
        }

    }
}
