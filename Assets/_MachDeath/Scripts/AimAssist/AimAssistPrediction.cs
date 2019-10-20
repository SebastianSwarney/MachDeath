using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimAssistPrediction : MonoBehaviour
{
    public Camera m_myCamera;
    public GameObject m_trackObject;
    private Rigidbody m_trackingRb;
    private Vector3 m_predictedPosition;
    public float m_spearSpeed;

    public UnityEngine.UI.Image m_aimAssistImage;
    public LayerMask m_obstructingLayer;

    public float m_predictionRadius;
    public float m_predictionLength;
    public LayerMask m_predictionLayer;

    public Transform m_fireSpot;

    public float m_maxAngleInRange;

    [Header("Debugging")]
    public bool m_isDebugging = true;
    public Color m_trackingColor;
    public Color m_predictionColor;
    public bool m_showLengths = true;

    public bool m_objectFound;
    public float m_spearGravity;
    public Transform m_bulletInstantiator;

    private void Update()
    {
        if (m_trackObject == null)
        {
            m_objectFound = false;
            SearchForObjects();
            ShowAssistElement(false);
        }
        else
        {
            m_objectFound = true;
            m_predictedPosition = PredictPosition(m_trackingRb.position, m_trackingRb.velocity, m_spearSpeed, m_spearGravity);

            m_bulletInstantiator.LookAt(m_predictedPosition);
            if (ObjectStillInRange(m_predictedPosition))
            {
                ShowAssistElement(true);
            }
            else
            {
                if (ObjectUnobstructed(m_predictedPosition))
                {
                    ShowAssistElement(true);
                }
                if (!ObjectStillInRange(m_trackObject.transform.position))
                {
                    
                    ResetAimAssist();
                }
            }
        }

    }

    private void SearchForObjects()
    {
        RaycastHit hit;
        
        if (Physics.SphereCast(m_fireSpot.position-(m_fireSpot.forward *.5f*m_predictionLength), m_predictionRadius, m_fireSpot.forward, out hit, m_predictionLength*1.5f, m_predictionLayer))
        {
            print("Hit");
            m_maxAngleInRange = Mathf.Tan(m_predictionRadius / m_predictionLength) * Mathf.Rad2Deg;
            if (ObjectStillInRange(hit.collider.transform.position))
            {
                m_trackObject = hit.collider.gameObject;
                m_trackingRb = m_trackObject.GetComponent<Rigidbody>();
            }
        }
    }

    public float dis, ang;
    private bool ObjectStillInRange(Vector3 p_pos)
    {
        dis = Vector3.Distance(p_pos, transform.position);
        if (Vector3.Distance(p_pos, transform.position) > m_predictionLength)
        {
            print("Too Far");
            return false;
        }
        
        m_maxAngleInRange = Mathf.Tan(m_predictionRadius / m_predictionLength) * Mathf.Rad2Deg;
        ang = Vector3.Angle(p_pos - m_fireSpot.position, m_fireSpot.forward);
        if (Vector3.Angle(p_pos - m_fireSpot.position, m_fireSpot.forward) > m_maxAngleInRange)
        {
            print("Too Angled Out : ");
            return false;
        }


        return (ObjectUnobstructed(p_pos));

    }

    private void ResetAimAssist()
    {
        m_trackingRb = null;
        m_trackObject = null;
    }

    private bool ObjectUnobstructed(Vector3 p_pos)
    {
        bool obstructed = Physics.Linecast(p_pos, transform.position, m_obstructingLayer);
        if (obstructed)
        {
            print("Obstructed");
        }
        return !obstructed;
    }

    private void ShowAssistElement(bool p_active)
    {
        if (m_isDebugging)
        {
            if (m_trackingRb != null)
            {
                Debug.DrawLine(m_fireSpot.position, m_trackingRb.position, m_trackingColor);
                Debug.DrawLine(m_fireSpot.position, m_predictedPosition, m_predictionColor);
            }
        }
        m_aimAssistImage.gameObject.SetActive(p_active);
        m_aimAssistImage.transform.position = m_myCamera.WorldToScreenPoint(m_predictedPosition);
    }



    ///<Summary> 
    ///Uses physics to predict position.false Used for player aim assist
    ///<Summary> 
    private Vector3 PredictPosition(Vector3 p_targetPos, Vector3 p_targetVel, float p_projectileSpeed, float p_projectileGravity)
    {

        Vector3 displacment = p_targetPos - transform.position;
        float targetMoveAngle = Vector3.Angle(-displacment, p_targetVel) * Mathf.Deg2Rad;

        //The target is too fast, or has a zero velocity
        if (p_targetVel.magnitude == 0 || p_targetVel.magnitude > p_projectileSpeed && Mathf.Sin(targetMoveAngle) / p_projectileSpeed > Mathf.Cos(targetMoveAngle) / p_targetVel.magnitude)
        {
            print("No prediction");
            return p_targetPos;
        }

        float shootAngle = Mathf.Asin(Mathf.Sin(targetMoveAngle) * p_targetVel.magnitude / p_projectileSpeed);

        //Return the predicted position
        //TODO: Find out what this actually means later
        return p_targetPos + p_targetVel * displacment.magnitude / Mathf.Sin(Mathf.PI - targetMoveAngle - shootAngle) * Mathf.Sin(shootAngle) / p_targetVel.magnitude;
    }


    private void OnDrawGizmos()
    {
        if (!m_isDebugging || !m_showLengths) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(m_fireSpot.position, m_predictionRadius);
        Gizmos.DrawWireSphere(m_fireSpot.position + (m_fireSpot.forward * m_predictionLength), m_predictionRadius);
        Gizmos.DrawWireSphere(m_fireSpot.position + (m_fireSpot.forward * m_predictionLength/2), m_predictionRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(m_fireSpot.position, m_fireSpot.position + (m_fireSpot.forward * m_predictionLength));
        Gizmos.DrawLine(m_fireSpot.transform.position, m_fireSpot.transform.position + (Quaternion.AngleAxis(Mathf.Tan(m_predictionRadius / m_predictionLength) * Mathf.Rad2Deg, Vector3.right) * m_fireSpot.transform.forward * m_predictionLength));
        Gizmos.DrawLine(m_fireSpot.transform.position, m_fireSpot.transform.position + (Quaternion.AngleAxis(Mathf.Tan(m_predictionRadius / m_predictionLength) * Mathf.Rad2Deg, -Vector3.right) * m_fireSpot.transform.forward * m_predictionLength));
        Gizmos.DrawLine(m_fireSpot.transform.position, m_fireSpot.transform.position + (Quaternion.AngleAxis(Mathf.Tan(m_predictionRadius / m_predictionLength) * Mathf.Rad2Deg, Vector3.up) * m_fireSpot.transform.forward * m_predictionLength));
        Gizmos.DrawLine(m_fireSpot.transform.position, m_fireSpot.transform.position + (Quaternion.AngleAxis(Mathf.Tan(m_predictionRadius / m_predictionLength) * Mathf.Rad2Deg, -Vector3.up) * m_fireSpot.transform.forward * m_predictionLength));
    }




}
