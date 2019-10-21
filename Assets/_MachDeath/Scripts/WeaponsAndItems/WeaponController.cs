using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public bool isSpear, isActive;

    [SerializeField]
    private GameObject hitMarker;

    //Item properties
    [SerializeField]
    private List<GameObject> weapons = new List<GameObject>();

    [SerializeField]
    private List<GameObject> utility = new List<GameObject>();

    [SerializeField]
    private List<GameObject> misc = new List<GameObject>();

    //[SerializeField]
    public List<GameObject> usableItemsRightClick = new List<GameObject>();

    public List<GameObject> usableItemsLeftClick = new List<GameObject>();

    //These should be in weapon stats
    private float firingSince, fireDelay;

    //Set current weapon index to 0 at start
    //private int currentItemIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        filterChildren();
    }

    private void filterChildren()
    {
        float childCount = transform.childCount;

        for (int i = 1; i <= childCount; i++)
        {

            GameObject child = transform.GetChild(i - 1).gameObject;

            if(child.GetComponent<WeaponBase>())
            {
                weapons.Add(child);
                PlaceItems(child, usableItemsLeftClick);
            }

            if (child.GetComponent<UtilityBase>())
            {
                utility.Add(child);
                PlaceItems(child, usableItemsRightClick);
            }

            if (child.GetComponent<MiscBase>())
            {
                misc.Add(child);
                PlaceItems(child, usableItemsLeftClick);
            }
        }
    }

    private void PlaceItems(GameObject Item, List<GameObject> List)
    {
        List.Add(Item);
    }

    // Update is called once per frame
    void Update()
    {

    }
}

public class Spear : WeaponBase
{

}

public class Shield : WeaponBase
{

}
