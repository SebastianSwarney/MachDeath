using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SpearRicochet : MonoBehaviour
{
    public LayerMask collisionMask;

    public float speed, reflections, maxReflectionDistance;

    //[SerializeField]
    //private LayerMask layermask;

    private Collider spearCollider;

    private Rigidbody m_rb;

    private bool isRayHit;


    // Start is called before the first frame update
    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        spearCollider = GetComponent<Collider>();
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
        //Handles.color = Color.red;
        //Handles.ArrowHandleCap(0, this.transform.position + this.transform.forward * 0.25f, this.transform.rotation, 0.5f, EventType.Repaint);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, 0.25f);
    }

    private void DetectHit()
    {
        if (reflections > 0)
        {
            Ray ray = new Ray(this.transform.position, this.transform.forward * 3f);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 2.0f))
            {
                RayHitNew(this.transform.position + this.transform.forward * 0.75f, this.transform.forward, (int)reflections);
            }
        }
        else
        {
            spearCollider.enabled = true;
            //m_rb.velocity = Vector3.zero;
        }
    }

    private void RayHitNew(Vector3 position, Vector3 direction, int reflectionsRemaining)
    {

        Debug.Log("Reflection: " + reflectionsRemaining);
        if (reflectionsRemaining == 0)
        {
            Debug.Log("Returning!");
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

            reflections--;

            Invoke("ResetIsHit", 0.15f);
            GetComponent<Rigidbody>().velocity = transform.forward * 20f;
            //transform.rotation.SetFromToRotation(transform.forward, GetComponent<Rigidbody>().velocity);
            //Debug.Log("current spear velocity " + GetComponent<Rigidbody>().velocity);
        }
        else
        {
            position += direction * maxReflectionDistance;
        }

        RayHitNew(position, direction, reflectionsRemaining - 1);
    }

    private void ResetIsHit()
    {
        isRayHit = false;
    }
}
