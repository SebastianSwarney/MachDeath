using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearThrower : MonoBehaviour
{
    public GameObject m_spear;

    public float m_throwSpeed;

    private Camera m_camera;

    private void Start()
    {
        m_camera = GetComponentInChildren<Camera>();
    }

    private void Update()
    {
        DrawLine();

        if (Input.GetMouseButtonDown(0))
        {
            ThrowSpear();
        }
    }

    private void ThrowSpear()
    {
        GameObject newSpear = Instantiate(m_spear, (m_camera.transform.position + m_camera.transform.forward * 3), Quaternion.LookRotation(m_camera.transform.forward, Vector3.up));
        newSpear.GetComponent<Rigidbody>().AddForce(m_camera.transform.forward * m_throwSpeed, ForceMode.Impulse);
    }

    private void DrawLine()
    {
        RaycastHit hit;

        if (Physics.Raycast(m_camera.transform.position, m_camera.transform.forward, out hit, Mathf.Infinity))
        {
            Vector3 reflectDir = Vector3.Reflect(m_camera.transform.forward, hit.normal);

            Debug.DrawLine(m_camera.transform.position, hit.point, Color.red);

            Debug.DrawLine(hit.point, hit.point + hit.normal, Color.blue);

            Debug.DrawLine(hit.point, (hit.point + reflectDir));
        }
    }
}
