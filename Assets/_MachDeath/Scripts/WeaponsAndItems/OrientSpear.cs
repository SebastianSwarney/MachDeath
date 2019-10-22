using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrientSpear : MonoBehaviour
{
    [SerializeField]
    private Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        getDirection();
    }

    void getDirection()
    {
        //this.transform.rotation = transform.parent.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
