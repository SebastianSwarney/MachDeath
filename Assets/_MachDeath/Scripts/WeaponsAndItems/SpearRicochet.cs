using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[System.Serializable]
public class SpearCollisionEvent: UnityEngine.Events.UnityEvent { }

public class SpearRicochet : MonoBehaviour
{
    public LayerMask collisionMask;

    public float speed, reflections,m_maxReflections, maxReflectionDistance;

    //[SerializeField]
    //private LayerMask layermask;

    private Collider spearCollider;

    private Rigidbody m_rb;

    private bool isRayHit;

    public SpearCollisionEvent m_spearCollision;


    [Header("Projectile Properties")]
    public float m_sphereCastRadius;


    [Header("Debugging")]
    public bool m_debugging;
    public Color m_debuggingColor;

    // Start is called before the first frame update
    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        
        isRayHit = false;
    }
    private void OnEnable()
    {
        reflections = m_maxReflections;
        if (spearCollider == null)
        {
            spearCollider = GetComponent<Collider>();
        }
        spearCollider.enabled = false;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.DrawLine(m_rb.position, m_rb.position + transform.forward*m_rb.velocity.magnitude*Time.deltaTime, Color.magenta);

        if (reflections > 0)
        {


            RaycastHit hit;
            if (Physics.Raycast(m_rb.position, transform.forward, out hit, m_rb.velocity.magnitude * Time.deltaTime, collisionMask))
            {

                float distance = (hit.point - m_rb.position).magnitude - 3f;
                m_rb.position = (m_rb.velocity.normalized * distance) + m_rb.position;
                print("Hit");


            }
        }
        DetectHit();

        transform.LookAt(transform.position + m_rb.velocity);
    }

    private void OnDrawGizmos()
    {
        if (!m_debugging) return;
        //Handles.color = Color.red;
        //Handles.ArrowHandleCap(0, this.transform.position + this.transform.forward * 0.25f, this.transform.rotation, 0.5f, EventType.Repaint);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, 0.25f);


        
        Gizmos.color = m_debuggingColor;
        Gizmos.DrawWireSphere(transform.position, m_sphereCastRadius);
        

    }

    private void DetectHit()
    {
        if (reflections > 0)
        {
            Ray ray = new Ray(this.transform.position, this.transform.forward * 3f);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 2.0f,collisionMask))
            {
                RayHitNew(this.transform.position + this.transform.forward * 0.75f, this.transform.forward, (int)reflections);
            }
        }
        else
        {
            spearCollider.enabled = true;
        }
    }

    private void RayHitNew(Vector3 position, Vector3 direction, int reflectionsRemaining)
    {

        //Debug.Log("Reflection: " + reflectionsRemaining);
        if (reflectionsRemaining == 0)
        {
            ///Debug.Log("Returning!");
            return;
        }

        Vector3 startingPos = position;

        Ray ray = new Ray(position, direction);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxReflectionDistance, collisionMask) && !isRayHit)
        {
            isRayHit = true;
            direction = Vector3.Reflect(direction, hit.normal);
            position = hit.point;
            
            transform.LookAt(transform.position + direction.normalized * 10);

            reflections--;

            Invoke("ResetIsHit", 0.15f);
            GetComponent<Rigidbody>().velocity = transform.forward * 20f;

        }
        else
        {
            position += direction * maxReflectionDistance;
        }

        
    }

    private void ResetIsHit()
    {
        isRayHit = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        m_spearCollision?.Invoke();
    }

}
