using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityBase : ItemBase
{
    [SerializeField]
    private Transform shieldActive, shieldInactive;

    [SerializeField]
    private float LerpTimer;

    [SerializeField]
    private float LerpSpeed, shieldOffset;

    [SerializeField]
    private AnimationCurve animationCurve;

    [SerializeField]
    private bool isShieldRaised;

    private Vector3 defefaultShield;

    void Start()
    {
        GetItemType();
        GetItemData();
        isShieldRaised = false;
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
        if (!isShieldRaised)
        {
            Debug.Log("Using Shield");
            LerpTimer = 0;
            isShieldRaised = true;
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
        this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, defefaultShield, animationCurve.Evaluate(((LerpTimer) * 3 * LerpSpeed)));
    }

    private IEnumerator WaitToSheath()
    {
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Resetting Shield");
        isShieldRaised = false;
        LerpTimer = 0;
    }
}


