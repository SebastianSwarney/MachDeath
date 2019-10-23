using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SpearRicochet : MonoBehaviour
{
    public LayerMask collisionMask;

    public float speed, reflections, maxReflectionDistance;

    // Start is called before the first frame update
    void Start()
    {
        RayHitNew(this.transform.position + this.transform.forward * 0.75f, this.transform.forward, (int)reflections);
    }

    // Update is called once per frame
    void Update()
    {
        //RayHit();
    }

    //Deprecated Method
    private void RayHit()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Time.deltaTime * speed + 0.1f, collisionMask))
        {
            Vector3 reflectDir = Vector3.Reflect(ray.direction, hit.normal);

            float rot = 180 - Mathf.Atan2(reflectDir.z, reflectDir.x) * Mathf.Rad2Deg;
            float rot2 = 180 - Mathf.Atan2(reflectDir.z, reflectDir.y) * Mathf.Rad2Deg;
            float rot3 = 180 - Mathf.Atan2(reflectDir.y, reflectDir.x) * Mathf.Rad2Deg;

            Debug.DrawLine(transform.position, hit.point, Color.red);

            transform.eulerAngles = new Vector3(-rot2, -rot, -rot3);

            GetComponent<Rigidbody>().velocity = transform.forward * 60f;
            //transform.rotation 
        }
    }

    private void RayHitNew(Vector3 position, Vector3 direction, int reflectionsRemaining)
    {
        if (reflectionsRemaining <= 0)
        {
            return;
        }

        Vector3 startingPos = position;

        Ray ray = new Ray(position, direction);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxReflectionDistance))
        {
            direction = Vector3.Reflect(direction, hit.normal);
            position = hit.point;
            GetComponent<Transform>().rotation = Quaternion.Euler(this.transform.forward + direction);
            GetComponent<Rigidbody>().velocity = direction * 60f;
        }
        else
        {
            position += direction * maxReflectionDistance;
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(position, position + direction * maxReflectionDistance);

        RayHitNew(position, direction, reflectionsRemaining - 1);
    }

    private void OnDrawGizmos()
    {
        Handles.color = Color.red;
        Handles.ArrowHandleCap(0, this.transform.position + this.transform.forward * 0.25f, this.transform.rotation, 0.5f, EventType.Repaint);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, 0.25f);
    }
}
