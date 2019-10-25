using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponController : MonoBehaviour
{
    //public bool isSpear, isActive;

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

    public bool itemInUse;

    //Shield Regen Params
    public float shieldRegenerateTimer;

    public Image shieldRegenCounter;

    //Set current weapon index to 0 at start
    //private int currentItemIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        filterChildren();
        itemInUse = false;
        //Dont forget to drag and drop this
        shieldRegenCounter.gameObject.SetActive(false);
    }

    private void filterChildren()
    {
        float childCount = transform.childCount;

        for (int i = 1; i <= childCount; i++)
        {

            GameObject child = transform.GetChild(i - 1).gameObject;

            if (child.GetComponent<WeaponBase>())
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

    public void DeActivateShield()
    {
        //Shut down Shield Here
        var ShieldObj = utility[0].gameObject;
        itemInUse = false;
        ShieldObj.SetActive(false);
        StartCoroutine(Countdown());
    }

    private IEnumerator Countdown()
    {
        float duration = shieldRegenerateTimer;
        // 3 seconds you can change this to
        //to whatever you want
        float totalTime = 0;

        while (totalTime <= duration)
        {
            Debug.Log("Shield Regenerate Timer Started!");
            shieldRegenCounter.gameObject.SetActive(true);
            shieldRegenCounter.fillAmount = totalTime / duration;
            totalTime += Time.deltaTime;
            //var integer = (int)totalTime; /* choose how to quantize this */
            /* convert integer to string and assign to text */
            yield return null;
        }

        var ShieldObj = utility[0].gameObject;
        shieldRegenCounter.gameObject.SetActive(false);
        ShieldObj.SetActive(true);
        ShieldObj.GetComponentInChildren<ShieldHealthCheck>().ResetHealth();
        ShieldObj.GetComponent<UtilityBase>().Start();
        ShieldObj.GetComponent<UtilityBase>().LerpTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
