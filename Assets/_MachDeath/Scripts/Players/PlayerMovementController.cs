using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class PlayerControllerEvent : UnityEvent { }


public class PlayerMovementController : MonoBehaviour
{
    public float mouseSensitivity;
    public float maxCameraAng;
    public bool m_inverted;
    public Transform playerBody;
    public Camera m_camera;

    private float xAxisClamp;

    public float m_baseMovementSpeed;
    public float m_runningMovementSpeeed;
    public float m_accelerationTime;

    private float m_currentMovementSpeed;

    public float m_maxJumpHeight = 4;
    public float m_minJumpHeight = 1;
    public float m_timeToJumpApex = .4f;

    private int m_jumpCount;
    private float m_gravity;
    private float m_maxJumpVelocity;
    private float m_minJumpVelocity;

    public float m_graceTime;
    private float m_graceTimer;

    public float m_jumpBufferTime;
    private float m_jumpBufferTimer;

    public AnimationCurve m_leapCurve;
    public float m_leapTime;
    public float m_leapSpeedBoostBase;
    public float m_leapSpeedBoostIncrease;
    public int m_leapSpeedBoostCountMax;
    public float m_leapBufferTime;

    public float m_currentLeapTime;
    public float m_leapingTimer;
    private float m_leapBufferTimer;
    public int m_leapCount;

    public float m_wallRunStickDistance;

    private bool m_isLanded;
    private bool m_isRunning;
    private bool m_isLeaping;
    private Vector3 m_velocity;
    private Vector3 m_velocitySmoothing;
    private CharacterController m_characterController;

    public Vector3 m_impact;
    public Rigidbody m_rb;
    public float shit;
    private bool m_isStunned;

    private Vector2 m_movementInput;
    private Vector2 m_lookInput;

    private void Start()
    {
        CalculateJump();

        m_characterController = GetComponent<CharacterController>();

        LockCursor();
        xAxisClamp = 0.0f;

        m_currentMovementSpeed = m_baseMovementSpeed;

        m_leapBufferTimer = m_leapBufferTime;
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
        if (Input.GetMouseButtonUp(0))
        {
            OnJumpInputUp();
        }

        if (!IsGrounded())
        {
            WallRunRay();
        }

        m_characterController.Move(m_velocity * Time.deltaTime);

        CalculateGroundPhysics();
        CameraRotation();
    }

    public void SetMovementInput(Vector2 p_input)
    {
        m_movementInput = p_input;
    }

    public void SetLookInput(Vector2 p_input)
    {
        m_lookInput = p_input;
    }

    private void WallRunRay()
    {
        RaycastHit hit;

        if (Physics.Raycast(m_characterController.transform.position, -m_characterController.transform.right, out hit, m_wallRunStickDistance))
        {
            if (Vector3.Dot(hit.normal, Vector3.up) == 0)
            {
                Vector3 wallVector = Vector3.Cross(hit.normal, Vector3.up);

                m_velocity = wallVector * m_baseMovementSpeed;
            }

            Debug.DrawLine(m_characterController.transform.position, hit.point);
        }
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

        if (IsGrounded() && !m_isLanded)
        {
            Landed();
        }

        if (!IsGrounded())
        {
            m_isLanded = false;
        }
    }

    private void Landed()
    {
        m_isLanded = true;
        m_leapBufferTimer = 0;
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

        Vector2 input = new Vector2(m_movementInput.x, m_movementInput.y);

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

        if (BunnyHop())
        {
            RunLeap();
            JumpMaxVelocity();
        }

        if (IsGrounded())
        {
            m_leapBufferTimer += Time.deltaTime;
        }

        if (IsGrounded() && m_leapBufferTimer > m_leapBufferTime)
        {
            m_leapCount = 0;
        }
    }

    private bool BunnyHop()
    {
        if (m_jumpBufferTimer > 0 && m_characterController.collisionFlags == CollisionFlags.Below)
        {
            m_jumpBufferTimer = 0;
            return true;
        }

        return false;
    }

    public void OnJumpInputDown()
    {
        if (m_leapBufferTimer <= m_leapBufferTime && IsGrounded())
        {
            RunLeap();
            m_leapBufferTimer = 0;
        }

        if (!IsGrounded())
        {
            m_jumpBufferTimer = m_jumpBufferTime;
        }

        if (IsGrounded())
        {
            //m_leapCount = 0;
            JumpMaxVelocity();
        }

        if (!IsGrounded() && m_graceTimer <= m_graceTime && m_velocity.y <= 0)
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

    private void RunLeap()
    {
        m_leapCount++;

        if (m_isLeaping)
        {
            //m_currentLeapTime += m_leapTime;
            m_leapingTimer = 0;
        }
        else
        {
            StartCoroutine(JumpLeap());
        }
    }

    private IEnumerator JumpLeap()
    {
        m_isLeaping = true;
        m_leapingTimer = 0;

        m_currentLeapTime = m_leapTime;

        float m_leapSpeed;

        while (m_leapingTimer < m_currentLeapTime)
        {
            m_leapingTimer += Time.deltaTime;

            if (m_leapCount <= m_leapSpeedBoostCountMax)
            {
                m_leapSpeed = m_leapSpeedBoostBase + (m_leapSpeedBoostIncrease * m_leapCount);
            }
            else
            {
                m_leapSpeed = m_leapSpeedBoostBase + (m_leapSpeedBoostIncrease * m_leapSpeedBoostCountMax);
            }

            float progress = m_leapCurve.Evaluate(m_leapingTimer / m_currentLeapTime);

            m_currentMovementSpeed = Mathf.Lerp(m_currentMovementSpeed, m_leapSpeed, progress);

            yield return null;
        }

        m_currentMovementSpeed = m_baseMovementSpeed;

        m_isLeaping = false;
    }

    private bool IsGrounded()
    {
        if (m_characterController.collisionFlags == CollisionFlags.Below)
        {
            return true;
        }

        return false;
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
        //Get the inputs for the camera
        Vector2 cameraInput = new Vector2(m_lookInput.y * ((m_inverted) ? -1 : 1), m_lookInput.x);

        //Rotate the player on the y axis (left and right)
        transform.Rotate(Vector3.up, cameraInput.y * (mouseSensitivity));

        float cameraXAng = m_camera.transform.eulerAngles.x;



        //Stops the camera from rotating, if it hits the resrictions
        if (cameraInput.x < 0 && cameraXAng > 360 - maxCameraAng || cameraInput.x < 0 && cameraXAng < maxCameraAng + 10)
        {
            m_camera.transform.Rotate(Vector3.right, cameraInput.x * (mouseSensitivity));

        }
        else if (cameraInput.x > 0 && cameraXAng > 360 - maxCameraAng - 10 || cameraInput.x > 0 && cameraXAng < maxCameraAng)
        {
            m_camera.transform.Rotate(Vector3.right, cameraInput.x * (mouseSensitivity));

        }

        if (m_camera.transform.eulerAngles.x < 360 - maxCameraAng && m_camera.transform.eulerAngles.x > 180)
        {
            m_camera.transform.localEulerAngles = new Vector3(360 - maxCameraAng, 0f, 0f);
        }
        else if (m_camera.transform.eulerAngles.x > maxCameraAng && m_camera.transform.eulerAngles.x < 180)
        {
            m_camera.transform.localEulerAngles = new Vector3(maxCameraAng, 0f, 0f);
        }
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
