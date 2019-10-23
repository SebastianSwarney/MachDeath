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
    private float LerpSpeed;

    [SerializeField]
    private AnimationCurve animationCurve;

    private Vector3 defefaultShield;

    void Start()
    {
        GetItemType();
        GetItemData();
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
        Debug.Log("Using Shield");
        LerpTimer = 0;
        //this.transform.position = Vector3.Lerp(defefaultShield, defefaultShield + offset, animationCurve.Evaluate((LerpTimer * LerpSpeed)));
    }
    // Start is called before the first frame update
  

    // Update is called once per frame
    void Update()
    {
        LerpTimer += Time.deltaTime;
        Vector3 offset = new Vector3(0.2f, 0, 0);
        this.transform.localPosition = Vector3.Lerp(defefaultShield, defefaultShield + offset, animationCurve.Evaluate((LerpTimer * LerpSpeed)));
    }
}
