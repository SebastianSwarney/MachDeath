using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMouseItemsInput : MonoBehaviour
{
    [SerializeField]
    private WeaponController weaponController;

    //start the index on 0
    private int currentUsableIndex = 0;


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
            Items[currentUsableIndex].gameObject.SetActive(false);
            currentUsableIndex = (currentUsableIndex + 1) % (Items.Count);
            Items[currentUsableIndex].gameObject.SetActive(true);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < .0f)
        {
            Items[currentUsableIndex].gameObject.SetActive(false);

            if (currentUsableIndex == 0)
            {
                currentUsableIndex = Items.Count - 1;
            }
            else
            {
                currentUsableIndex = (currentUsableIndex - 1) % (Items.Count);
            }

            Items[currentUsableIndex].gameObject.SetActive(true);
        }
    }

    void ItemUse()
    {
        var Items = weaponController.usableItemsLeftClick;

        if (Items[currentUsableIndex])
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Items[currentUsableIndex].GetComponent<WeaponBase>().UseItem(0f);
            }
            else
            {
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

        if (Items2[currentUsableIndex])
        {
            if (Input.GetButtonDown("Fire2"))
            {
                Items2[currentUsableIndex].GetComponent<UtilityBase>().UseItem(0f);
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
