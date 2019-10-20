using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    public string horizontalInputName;
    public string verticalInputName;

    public string mouseXInputName, mouseYInputName;
    public float mouseSensitivity;

    public float m_walkingMovementSpeed;

    public float m_runningMovementSpeeed;

    public float m_accelerationTime;

    private float m_currentMovementSpeed;

    public float m_maxJumpHeight = 4;
    public float m_minJumpHeight = 1;
    public float m_timeToJumpApex = .4f;

    private float m_gravity;
    private float m_maxJumpVelocity;
    private float m_minJumpVelocity;


    private Vector3 m_velocity;
    private Vector3 m_velocitySmoothing;

    private CharacterController m_characterController;
    public Transform playerBody;
    public Camera m_camera;

    private float xAxisClamp;
    private bool isJumping;

    public Vector3 m_impact;

    public Rigidbody m_rb;

    public float shit;

    private bool m_isStunned;

    public float m_graceTime;
    private float m_graceTimer;

    public float m_jumpBufferTime;
    private float m_jumpBufferTimer;

    private bool m_isLanded;

    private bool m_isRunning;

    public AnimationCurve m_leapCurve;
    public float m_leapTime;
    public float m_leapSpeedBoostMax;

    private void Start()
    {
        CalculateJump();

        m_characterController = GetComponent<CharacterController>();

        LockCursor();
        xAxisClamp = 0.0f;
    }

    private void OnValidate()
    {
        CalculateJump();
    }

    private void Update()
    {
        InputBuffering();
        CalculateVelocity();


        if (Input.GetMouseButtonDown(0))
        {
            OnJumpInputDown();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnJumpInputDown();
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            OnJumpInputUp();
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            m_currentMovementSpeed = m_runningMovementSpeeed;
            m_isRunning = true;
        }
        else
        {
            m_currentMovementSpeed = m_walkingMovementSpeed;
            m_isRunning = false;
        }

        m_characterController.Move(m_velocity * Time.deltaTime);

        CalculateGroundPhysics();
        CameraRotation();
    }

    private void CalculateGroundPhysics()
    {
        if (m_characterController.collisionFlags == CollisionFlags.Below && !OnSlope())
        {
            m_velocity.y = 0;
        }

        if (OnSlope())
        {
            RaycastHit hit;

            Vector3 bottom = m_characterController.transform.position - new Vector3(0, m_characterController.height / 2, 0);

            if (Physics.Raycast(bottom, Vector3.down, out hit))
            {
                m_characterController.Move(new Vector3(0, -(hit.distance), 0));
            }
        }
    }

    private void CalculateJump()
    {
        m_gravity = -(2 * m_maxJumpHeight) / Mathf.Pow(m_timeToJumpApex, 2);
        m_maxJumpVelocity = Mathf.Abs(m_gravity) * m_timeToJumpApex;
        m_minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(m_gravity) * m_minJumpHeight);
    }

    private void CalculateVelocity()
    {
        m_velocity.y += m_gravity * Time.deltaTime;

        Vector2 input = new Vector2(Input.GetAxisRaw(horizontalInputName), Input.GetAxisRaw(verticalInputName));

        Vector3 forwardMovement = transform.forward * input.y;
        Vector3 rightMovement = transform.right * input.x;

        Vector3 targetHorizontalMovement = Vector3.ClampMagnitude(forwardMovement + rightMovement, 1.0f) * m_currentMovementSpeed;
        Vector3 horizontalMovement = Vector3.SmoothDamp(m_velocity, targetHorizontalMovement, ref m_velocitySmoothing, m_accelerationTime);

        m_velocity = new Vector3(horizontalMovement.x, m_velocity.y, horizontalMovement.z);
    }

    private void InputBuffering()
    {
        if (m_characterController.collisionFlags == CollisionFlags.Below)
        {
            m_graceTimer = 0;
        }

        if (!(m_characterController.collisionFlags == CollisionFlags.Below))
        {
            m_graceTimer += Time.deltaTime;
        }

        if (m_jumpBufferTimer > 0)
        {
            m_jumpBufferTimer -= Time.deltaTime;
        }

        if (m_jumpBufferTimer > 0 && m_characterController.collisionFlags == CollisionFlags.Below)
        {
            m_jumpBufferTimer = 0;

            JumpMaxVelocity();

        }
    }

    public void OnJumpInputDown()
    {
        if (m_isRunning)
        {
            StartCoroutine(JumpLeap());
        }

        m_jumpBufferTimer = m_jumpBufferTime;

        if (!(m_characterController.collisionFlags == CollisionFlags.Below) && m_graceTimer <= m_graceTime && m_velocity.y <= 0)
        {
            m_graceTimer = m_graceTime;
            JumpMaxVelocity();



        }
    }

    public void OnJumpInputUp()
    {
        m_jumpBufferTimer = 0;

        if (m_velocity.y > m_minJumpVelocity)
        {
            JumpMinVelocity();
        }
    }

    public void JumpMaxVelocity()
    {
        m_velocity.y = m_maxJumpVelocity;
    }

    public void JumpMinVelocity()
    {
        m_velocity.y = m_minJumpVelocity;
    }

    private IEnumerator JumpLeap()
    {
        float t = 0;

        while (t < m_leapTime)
        {
            t += Time.deltaTime;

            float progress = m_leapCurve.Evaluate(t / m_leapTime);

            m_currentMovementSpeed = Mathf.Lerp(m_currentMovementSpeed, m_leapSpeedBoostMax, progress);

            yield return null;
        }
    }

    private bool OnSlope()
    {
        RaycastHit hit;

        Vector3 bottom = m_characterController.transform.position - new Vector3(0, m_characterController.height / 2, 0);

        if (Physics.Raycast(bottom, Vector3.down, out hit, 0.2f))
        {
            if (hit.normal != Vector3.up)
            {
                return true;
            }
        }

        return false;
    }

    private IEnumerator KnockBack()
    {
        m_isStunned = true;

        m_characterController.enabled = false;
        m_rb.isKinematic = false;

        m_rb.AddForce(-m_camera.transform.forward * shit, ForceMode.Impulse);

        while (m_isStunned)
        {
            yield return null;
        }

        m_rb.isKinematic = true;
        m_characterController.enabled = true;
    }

    #region Camera
    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }


    private void CameraRotation()
    {
        float mouseX = Input.GetAxis(mouseXInputName) * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis(mouseYInputName) * mouseSensitivity * Time.deltaTime;

        xAxisClamp += mouseY;

        if (xAxisClamp > 90.0f)
        {
            xAxisClamp = 90.0f;
            mouseY = 0.0f;
            ClampXAxisRotationToValue(270.0f);
        }
        else if (xAxisClamp < -90.0f)
        {
            xAxisClamp = -90.0f;
            mouseY = 0.0f;
            ClampXAxisRotationToValue(90.0f);
        }

        m_camera.transform.Rotate(Vector3.left * mouseY);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    private void ClampXAxisRotationToValue(float value)
    {
        Vector3 eulerRotation = transform.eulerAngles;
        eulerRotation.x = value;
        transform.eulerAngles = eulerRotation;
    }
    #endregion

    private void OnCollisionEnter(Collision collision)
    {
        m_isStunned = false;
    }
}
