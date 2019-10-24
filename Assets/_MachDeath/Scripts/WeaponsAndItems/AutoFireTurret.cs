using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoFireTurret : MonoBehaviour
{

    public SpearEvent m_throwSpear;
    //Networking THings    public SpearEvent m_throwSpear;

    public float coolDown, fireDelay;

    public GameObject spear;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FireDelay());
    }

    // Update is called once per frame
    void Update()
    {


    }

    private void OnEnable()
    {
        StartCoroutine(FireDelay());
    }

    private IEnumerator FireDelay()
    {
        yield return new WaitForSeconds(fireDelay);
        StartCoroutine(CountDown());
    }

    private IEnumerator CountDown()
    {
        float duration = coolDown;

        float totalTime = 0;

        while (totalTime < duration)
        {
            totalTime += Time.deltaTime;
            yield return null;
        }
        Debug.Log("Firing Gatling");
        ShootGun();
    }

    private void ShootGun()
    {
        //CalculateShot();
        //this.transform.LookAt((CalculateShot().point) + new Vector3(0,-90,0));

        //GameObject SpawnedSpear = Instantiate(spear);
        //SpawnedSpear.transform.position = this.transform.GetChild(0).position;
        //SpawnedSpear.transform.localRotation = this.transform.GetChild(0).localRotation;

        m_throwSpear?.Invoke();

        //this.transform.LookAt((CalculateShot()));
        StartCoroutine(CountDown());
    }
}
