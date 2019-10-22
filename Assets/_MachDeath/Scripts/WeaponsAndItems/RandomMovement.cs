using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovement : MonoBehaviour
{
    [SerializeField]
    private Transform[] Locations;

    private int moveIndex = 0;
    private float minDistance = 3f;
    private float speed;

    // Start is called before the first frame update
    void Start()
    {
        EjectChildren();
        speed = Random.Range(4f, 10f);
    }

    void EjectChildren()
    {
        for (int i = 0; i < Locations.Length; i++)
        {
            Locations[i].transform.parent = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        MoveBetweenLocations();
        CheckDistance();
    }

    private void MoveBetweenLocations()
    {
        float time = Time.deltaTime * speed;
        transform.position = Vector3.MoveTowards(this.transform.position, Locations[moveIndex].position, time);
    }

    private void CheckDistance()
    {
        if(Vector3.Distance(this.transform.position, Locations[moveIndex].position) < minDistance)
        {
            SelectLocations();
        }
    }

    private void SelectLocations()
    {
        moveIndex = (moveIndex + 1) % (Locations.Length);
    }
}
