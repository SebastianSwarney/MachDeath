using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoFireTurret : MonoBehaviour
{

    public SpearEvent m_throwSpear;
    //Networking THings    public SpearEvent m_throwSpear;

    public float coolDown;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CountDown());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator CountDown()
    {
        float duration = coolDown;

        float totalTime = 0;
        //while (true)
        //{
        while (totalTime < duration)
        {
            totalTime += Time.deltaTime;
            yield return null;
        }
        ShootGun();
        duration = coolDown;
        //}
    }

    private void ShootGun()
    {
        //CalculateShot();
        //this.transform.LookAt((CalculateShot().point) + new Vector3(0,-90,0));

        m_throwSpear.Invoke();
        //this.transform.LookAt((CalculateShot()));
    }
}
