using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class PlayerControllerEvent : UnityEvent { }


public class PlayerMovementController : MonoBehaviour
{
    public enum MovementControllState { MovementEnabled, MovementDisabled }
    public enum GravityState { GravityEnabled, GravityDisabled }
    public enum DamageState { Vulnerable, Invulnerable }
    public enum InputState { InputEnabled, InputDisabled }
    public PlayerState m_states;

    public float m_mouseSensitivity;
    public float m_maxCameraAng;
    public bool m_inverted;
    public Transform playerBody;
    public Camera m_camera;

    public float m_baseMovementSpeed;
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

    private float m_currentLeapTime;
    private float m_leapingTimer;
    private float m_leapBufferTimer;
    private int m_leapCount;
    private float m_currentLeapSpeed;

    public AnimationCurve m_wallSpeedCurve;
    public float m_wallSpeedUpTime;

    public float m_maxWallRunSpeed;
    public float m_wallRunCameraMaxTilt;

    public int m_rayCount;
    public float m_raySpacing;
    public float m_wallRunStickDistance;
    public float m_wallRunBufferTime;
    public Vector2 m_wallJumpOffVelocity;

    private float m_wallRunBufferTimer;
    private Vector3 m_wallDir;

    private float m_currentWallRunningSpeed;

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

    public Transform m_cameraTilt;
    public Transform m_cameraMain;
    private float m_tiltTarget;
    public float m_tiltSpeed;

    private float m_tiltSmoothingVelocity;

    private bool m_isWallRunning;

    private Vector3 m_wallVector;
    private Vector3 m_wallFacingVector;

    public LayerMask m_wallMask;

    private void Start()
    {
        CalculateJump();

        m_characterController = GetComponent<CharacterController>();

        LockCursor();

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



        CalculateCurrentSpeed();

        if (!IsGrounded())
        {
            WallRunRay();
        }

        CalculateVelocity();

        if (Input.GetMouseButtonDown(0))
        {
            OnJumpInputDown();
        }
        if (Input.GetMouseButtonUp(0))
        {
            OnJumpInputUp();
        }

        m_characterController.Move(m_velocity * Time.deltaTime);

        CalculateGroundPhysics();

        CameraRotation();

        TiltLerp();

    }

    public void SetMovementInput(Vector2 p_input)
    {
        m_movementInput = p_input;
    }

    public void SetLookInput(Vector2 p_input)
    {
        m_lookInput = p_input;
    }

    private void CalculateCurrentSpeed()
    {
        float speed = m_baseMovementSpeed;

        speed += m_currentLeapSpeed;

        speed += m_currentWallRunningSpeed;

        m_currentMovementSpeed = speed;

    }

    private void WallRunRay()
    {
        float m_angleBetweenRays = m_raySpacing / m_rayCount;

        bool rayHit = false;

        for (int i = 0; i < m_rayCount; i++)
        {
            
            Quaternion raySpaceQ = Quaternion.Euler(0, (i * m_angleBetweenRays) - (m_angleBetweenRays * (m_rayCount / 2)), 0);

            RaycastHit hit;

            if (Physics.Raycast(m_characterController.transform.position, raySpaceQ * transform.forward, out hit, m_wallRunStickDistance, m_wallMask))
            {

                if (Vector3.Dot(hit.normal, Vector3.up) == 0)
                {
                    rayHit = true;

                    m_wallVector = Vector3.Cross(hit.normal, Vector3.up);
                    m_wallFacingVector = Vector3.Cross(hit.normal, m_camera.transform.forward);

                    m_wallDir = hit.normal;

                    StartWallRun();
                }

                Debug.DrawLine(m_characterController.transform.position, hit.point);
            }
        }

        if (!rayHit)
        {
            m_isWallRunning = false;
        }

    }

    private void TiltLerp()
    {
        m_cameraTilt.localRotation = Quaternion.Slerp(m_cameraTilt.localRotation, Quaternion.Euler(0, 0, m_tiltTarget), m_tiltSpeed);
    }

    private void StartWallRun()
    {
        if (!m_isWallRunning)
        {
            if (m_wallRunBufferTimer >= m_wallRunBufferTime)
            {
                StartCoroutine(WallRunning());
                m_wallRunBufferTimer = 0;
            }
        }
    }

    IEnumerator WallRunning()
    {
        m_isWallRunning = true;

        m_states.m_gravityControllState = GravityState.GravityDisabled;

        float t = 0;

        m_states.m_movementControllState = MovementControllState.MovementDisabled;

        while (m_isWallRunning)
        {
            m_leapingTimer = 0;

            float result = Mathf.Lerp(-m_wallRunCameraMaxTilt, m_wallRunCameraMaxTilt, m_wallFacingVector.y);
            m_tiltTarget = result;

            m_velocity = (m_wallVector * -m_wallFacingVector.y) * m_currentMovementSpeed;
            m_velocity.y = 0;

            t += Time.deltaTime;

            float progress =  m_wallSpeedCurve.Evaluate(t / m_wallSpeedUpTime);

            m_currentWallRunningSpeed = Mathf.Lerp(m_currentWallRunningSpeed, m_maxWallRunSpeed, progress);

            yield return null;
        }

        m_states.m_movementControllState = MovementControllState.MovementEnabled;

        m_currentWallRunningSpeed = 0;

        m_states.m_gravityControllState = GravityState.GravityEnabled;

        m_tiltTarget = 0f;
    }

    private void CalculateGroundPhysics()
    {
        if (IsGrounded() && !OnSlope())
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
        if (m_states.m_gravityControllState == GravityState.GravityEnabled)
        {
            m_velocity.y += m_gravity * Time.deltaTime;
        }

        if (m_states.m_movementControllState == MovementControllState.MovementEnabled)
        {

            Vector2 input = new Vector2(m_movementInput.x, m_movementInput.y);

            Vector3 forwardMovement = transform.forward * input.y;
            Vector3 rightMovement = transform.right * input.x;

            Vector3 targetHorizontalMovement = Vector3.ClampMagnitude(forwardMovement + rightMovement, 1.0f) * m_currentMovementSpeed;
            Vector3 horizontalMovement = Vector3.SmoothDamp(m_velocity, targetHorizontalMovement, ref m_velocitySmoothing, m_accelerationTime);

            m_velocity = new Vector3(horizontalMovement.x, m_velocity.y, horizontalMovement.z);
        }

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

        if (!m_isWallRunning)
        {
            m_wallRunBufferTimer += Time.deltaTime;
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

        if (!IsGrounded() && !m_isWallRunning)
        {
            m_jumpBufferTimer = m_jumpBufferTime;
        }

        if (m_isWallRunning)
        {
            WallJump();
        }

        if (IsGrounded())
        {
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

    public void WallJump()
    {
        m_isWallRunning = false;

        m_velocity = m_wallDir * m_wallJumpOffVelocity.x;
        
        m_velocity.y = m_wallJumpOffVelocity.y;
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

        float targetLeapSpeed;

        while (m_leapingTimer < m_currentLeapTime)
        {
            m_leapingTimer += Time.deltaTime;

            if (m_leapCount <= m_leapSpeedBoostCountMax)
            {
                targetLeapSpeed = m_leapSpeedBoostBase + (m_leapSpeedBoostIncrease * m_leapCount);
            }
            else
            {
                targetLeapSpeed = m_leapSpeedBoostBase + (m_leapSpeedBoostIncrease * m_leapSpeedBoostCountMax);
            }

            float progress = m_leapCurve.Evaluate(m_leapingTimer / m_currentLeapTime);
            m_currentLeapSpeed = Mathf.Lerp(m_currentLeapSpeed, targetLeapSpeed, progress);

            yield return null;
        }

        m_currentLeapSpeed = 0;
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
        transform.Rotate(Vector3.up, cameraInput.y * (m_mouseSensitivity));

        float cameraXAng = m_cameraMain.transform.eulerAngles.x;



        //Stops the camera from rotating, if it hits the resrictions
        if (cameraInput.x < 0 && cameraXAng > 360 - m_maxCameraAng || cameraInput.x < 0 && cameraXAng < m_maxCameraAng + 10)
        {
            m_cameraMain.transform.Rotate(Vector3.right, cameraInput.x * (m_mouseSensitivity));

        }
        else if (cameraInput.x > 0 && cameraXAng > 360 - m_maxCameraAng - 10 || cameraInput.x > 0 && cameraXAng < m_maxCameraAng)
        {
            m_cameraMain.transform.Rotate(Vector3.right, cameraInput.x * (m_mouseSensitivity));

        }

        if (m_cameraMain.transform.eulerAngles.x < 360 - m_maxCameraAng && m_cameraMain.transform.eulerAngles.x > 180)
        {
            m_cameraMain.transform.localEulerAngles = new Vector3(360 - m_maxCameraAng, 0f, 0f);
        }
        else if (m_camera.transform.eulerAngles.x > m_maxCameraAng && m_cameraMain.transform.eulerAngles.x < 180)
        {
            m_cameraMain.transform.localEulerAngles = new Vector3(m_maxCameraAng, 0f, 0f);
        }
    }
    #endregion

    #region Player State Code
    [System.Serializable]
    public struct PlayerState
    {
        public MovementControllState m_movementControllState;
        public GravityState m_gravityControllState;
        public DamageState m_damageState;
        public InputState m_inputState;
    }
    #endregion


    private void OnCollisionEnter(Collision collision)
    {
        m_isStunned = false;
    }
}
