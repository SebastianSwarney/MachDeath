using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ShieldActiveEvent : UnityEngine.Events.UnityEvent<bool> { }
public class UtilityBase : ItemBase
{
    [HideInInspector]
    private Transform shieldActive, shieldInactive;

    [HideInInspector]
    public float LerpTimer;

    public float LerpSpeed, shieldOffset;

    [HideInInspector]
    public string spearTag;

    public GameObject spearPrefab;

    public AnimationCurve animationCurve;

    [HideInInspector]
    public bool isShieldRaised;

    [HideInInspector]
    public GameObject shieldCollider;

    private Vector3 defefaultShield;

    public ShieldActiveEvent m_shieldEvent;

    void Start()
    {
        GetItemType();
        GetItemData();
        isShieldRaised = false;
        shieldCollider = transform.GetChild(0).gameObject;
        shieldCollider.SetActive(false);
        spearTag = spearPrefab.gameObject.tag;
       
    }
    protected override void GetItemType()
    {
        ItemType = Item.utility;
    }
    protected override void GetItemData()
    {
        //Get reference to fps Camera and WeaponController//Should be item controller
        base.GetData();
        defefaultShield = this.transform.localPosition;
        LerpTimer = 0;
    }

    protected override void ApplyUseItem()
    {
        if (!isShieldRaised && !weaponController.itemInUse)
        {
            LerpTimer = 0;
            isShieldRaised = true;
            weaponController.itemInUse = true;
            m_shieldEvent.Invoke(true);
            //this.transform.position = Vector3.Lerp(defefaultShield, defefaultShield + offset, animationCurve.Evaluate((LerpTimer * LerpSpeed)));
        }
    }
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        //This is the visualization of the shield not the actual logic behind it
        if (isShieldRaised)
        {
            RaiseShield();
            //StartCoroutine(CountDown());
        }
        else
        {
            LowerShield();
        }
    }

    private void RaiseShield()
    {
        LerpTimer += Time.deltaTime;
        Vector3 offset = new Vector3(shieldOffset, 0, 0);
        this.transform.localPosition = Vector3.Lerp(defefaultShield, defefaultShield + offset, animationCurve.Evaluate((LerpTimer * 3 * LerpSpeed)));

        if ((LerpTimer * 3 * LerpSpeed) >= 1)
        {
            StartCoroutine(WaitToSheath());
        }
    }

    private void LowerShield()
    {
        LerpTimer += Time.deltaTime;
        Vector3 offset = new Vector3(shieldOffset, 0, 0);
        this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, defefaultShield, animationCurve.Evaluate(((LerpTimer) * 6 * LerpSpeed)));

        if ((LerpTimer * 6 * LerpSpeed) >= 1)
        {
            weaponController.itemInUse = false;
            m_shieldEvent.Invoke(false);
        }
    }

    private IEnumerator WaitToSheath()
    {
        yield return new WaitForSeconds(itemstats._itemCoolDown);

        isShieldRaised = false;
        LerpTimer = 0;
    }
}


