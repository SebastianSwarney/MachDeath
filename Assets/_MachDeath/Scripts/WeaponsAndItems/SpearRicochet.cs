using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SpearRicochet : MonoBehaviour
{
    public LayerMask collisionMask;

    public float speed, reflections, maxReflectionDistance;


    private Rigidbody m_rb;

    private bool isRayHit;


    // Start is called before the first frame update
    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        isRayHit = false;
    }

    // Update is called once per frame
    void Update()
    {
        //RayHit();
        //RayHitNew(this.transform.position + this.transform.forward * 0.75f, this.transform.forward, (int)reflections);
        DetectHit();

        transform.LookAt(transform.position + m_rb.velocity);
    }

    private void OnDrawGizmos()
    {
        Handles.color = Color.red;
        Handles.ArrowHandleCap(0, this.transform.position + this.transform.forward * 0.25f, this.transform.rotation, 0.5f, EventType.Repaint);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, 0.25f);
    }

    //Deprecated Method
    //private void RayHit()
    //{
    //    Ray ray = new Ray(transform.position, transform.forward);
    //    RaycastHit hit;

    //    if (Physics.Raycast(ray, out hit, Time.deltaTime * speed + 0.1f, collisionMask))
    //    {
    //        Vector3 reflectDir = Vector3.Reflect(ray.direction, hit.normal);

    //        float rot = 180 - Mathf.Atan2(reflectDir.z, reflectDir.x) * Mathf.Rad2Deg;
    //        float rot2 = 180 - Mathf.Atan2(reflectDir.z, reflectDir.y) * Mathf.Rad2Deg;
    //        float rot3 = 180 - Mathf.Atan2(reflectDir.y, reflectDir.x) * Mathf.Rad2Deg;

    //        Debug.DrawLine(transform.position, hit.point, Color.red);

    //        transform.eulerAngles = new Vector3(-rot2, -rot, -rot3);

    //        GetComponent<Rigidbody>().velocity = transform.forward * 60f;
    //        //transform.rotation 
    //    }
    //}

    private void DetectHit()
    {
        Ray ray = new Ray(this.transform.position, this.transform.forward * 3f);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 2.0f))
        {
            RayHitNew(this.transform.position + this.transform.forward * 0.75f, this.transform.forward, (int)reflections);
        }
    }




    private void RayHitNew(Vector3 position, Vector3 direction, int reflectionsRemaining)
    {

        Debug.Log("Reflection: " + reflectionsRemaining);
        if (reflectionsRemaining <= 0)
        {
            return;
        }

        Vector3 startingPos = position;

        Ray ray = new Ray(position, direction);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxReflectionDistance) && !isRayHit)
        {
            isRayHit = true;
            direction = Vector3.Reflect(direction, hit.normal);
            position = hit.point;
            Debug.DrawLine(transform.position, transform.position + direction, Color.magenta);

            //GetComponent<Transform>().rotation = Quaternion.Euler(direction);
            transform.LookAt(transform.position + direction.normalized * 10);

            GetComponent<Rigidbody>().velocity = transform.forward * 20f;
            //transform.rotation.SetFromToRotation(transform.forward, GetComponent<Rigidbody>().velocity);
            Debug.Log("current spear velocity " + GetComponent<Rigidbody>().velocity);
        }
        else
        {
            position += direction * maxReflectionDistance;
        }

        //Gizmos.color = Color.yellow;
        //Gizmos.DrawLine(position, position + direction * maxReflectionDistance);

        RayHitNew(position, direction, reflectionsRemaining - 1);
    }

}
