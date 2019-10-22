using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public class PlayerControllerEvent : UnityEvent { }

public class PlayerMovementController : MonoBehaviour
{
    public enum MovementControllState { MovementEnabled, MovementDisabled }
    public enum GravityState { GravityEnabled, GravityDisabled }
    public enum DamageState { Vulnerable, Invulnerable }
    public enum InputState { InputEnabled, InputDisabled }
    public PlayerState m_states;

    #region Camera Properties
    [Header("Camera Properties")]

    public float m_mouseSensitivity;
    public float m_maxCameraAng;
    public bool m_inverted;
    public Camera m_camera;
    public Transform m_cameraTilt;
    public Transform m_cameraMain;

    [Space]
    #endregion

    #region Base Movement Properties
    [Header("Base Movement Properties")]

    public float m_baseMovementSpeed;
    public float m_accelerationTime;

    private float m_currentMovementSpeed;
    private Vector3 m_velocity;
    private Vector3 m_velocitySmoothing;
    private CharacterController m_characterController;

    [Space]
    #endregion

    #region Jumping Properties
    [Header("Jumping Properties")]

    public float m_maxJumpHeight = 4;
    public float m_minJumpHeight = 1;
    public float m_timeToJumpApex = .4f;

    public float m_graceTime;
    private float m_graceTimer;

    public float m_jumpBufferTime;
    private float m_jumpBufferTimer;

    private float m_gravity;
    private float m_maxJumpVelocity;
    private float m_minJumpVelocity;
    private bool m_isLanded;
    private bool m_offLedge;

    [Space]
    #endregion

    #region Leaping Properties
    [Header("Leaping Properties")]

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
    private bool m_isLeaping;

    [Space]
    #endregion

    #region Wall Run Properties
    [Header("Wall Run Properties")]

    public LayerMask m_wallMask;

    public AnimationCurve m_wallSpeedCurve;
    public float m_wallSpeedUpTime;
    public float m_maxWallRunSpeed;

    public float m_tiltSpeed;
    public float m_wallRunCameraMaxTilt;

    public int m_wallRidingRayCount;
    public float m_wallRaySpacing;
    public float m_wallRunRayLength;
    public float m_wallRunBufferTime;
    public Vector3 m_wallRunJumpVelocity;

    public float m_wallJumpBufferTime;
    public Vector3 m_wallJumpVelocity;

    private float m_currentWallRunningSpeed;

    private float m_wallRunBufferTimer;
    private float m_wallJumpBufferTimer;

    private float m_tiltTarget;
    private float m_tiltSmoothingVelocity;

    private bool m_isWallRunning;
    public bool m_holdingWallRideStick;

    private Vector3 m_wallDir;
    private Vector3 m_wallVector;
    private Vector3 m_wallFacingVector;

    [Space]
    #endregion

    #region Slide Properties
    [Header("Slide Properties")]

    public float m_slideTime;
    public AnimationCurve m_slideCurve;
    public float m_slideSpeedUpTime;
    public float m_maxSlideSpeed;

    private float m_currentSlideSpeed;
    private bool m_isSliding;

    [Space]
    #endregion

    private Vector2 m_movementInput;
    private Vector2 m_lookInput;

    public Vector3 m_impact;
    public Rigidbody m_rb;
    public float shit;
    private bool m_isStunned;

    public Image m_speedometer;
    public AnimationCurve m_speedBarCurve;
    public float m_maxSpeed;



    private void Start()
    {
        CalculateJump();

        m_characterController = GetComponent<CharacterController>();

        LockCursor();

        m_currentMovementSpeed = m_baseMovementSpeed;


        m_jumpBufferTimer = m_jumpBufferTime;
        m_wallJumpBufferTimer = m_wallJumpBufferTime;
        m_wallRunBufferTimer = m_wallRunBufferTime;
    }

    private void OnValidate()
    {
        CalculateJump();
    }

    private void Update()
    {
        InputBuffering();

        CalculateCurrentSpeed();

        //FillSpeedBar();

        CheckWallRun();

        CalculateVelocity();

        m_characterController.Move(m_velocity * Time.deltaTime);

        CalculateGroundPhysics();

        CameraRotation();

        TiltLerp();
    }

    #region Input Code
    public void SetMovementInput(Vector2 p_input)
    {
        m_movementInput = p_input;
    }

    public void SetLookInput(Vector2 p_input)
    {
        m_lookInput = p_input;
    }

    public void WallRideInputDown()
    {
        m_holdingWallRideStick = true;
    }

    public void WallRideInputUp()
    {
        m_holdingWallRideStick = false;
        OnWallRideRelease();
    }
    #endregion

    #region Input Buffering Code

    private void InputBuffering()
    {
        if (!CheckBuffer(ref m_leapBufferTimer, ref m_leapBufferTime) && IsGrounded())
        {
            m_leapCount = 0;
        }

    }

    private bool CheckBuffer(ref float p_bufferTimer, ref float p_bufferTime)
    {
        if (p_bufferTimer < p_bufferTime)
        {
            p_bufferTimer = p_bufferTime;

            return true;
        }
        else if (p_bufferTimer >= p_bufferTime)
        {
            return false;
        }

        return false;
    }

    private bool CheckBuffer(ref float p_bufferTimer, ref float p_bufferTime, bool p_extraConditional)
    {
        if (p_bufferTimer < p_bufferTime)
        {
            if (p_extraConditional == false)
            {
                return false;
            }

            p_bufferTimer = p_bufferTime;

            return true;
        }
        else if (p_bufferTimer >= p_bufferTime)
        {
            return false;
        }

        return false;
    }


    private bool CheckBuffer(ref float p_bufferTimer, ref float p_bufferTime, bool[] p_extraConditionals)
    {
        if (p_bufferTimer < p_bufferTime)
        {
            for (int i = 0; i < p_extraConditionals.Length; i++)
            {
                if (p_extraConditionals[i] == false)
                {
                    return false;
                }
            }

            p_bufferTimer = p_bufferTime;

            return true;
        }
        else if (p_bufferTimer >= p_bufferTime)
        {
            return false;
        }

        return false;
    }

    private bool CheckOverBuffer(ref float p_bufferTimer, ref float p_bufferTime)
    {
        if (p_bufferTimer >= p_bufferTime)
        {
            p_bufferTimer = p_bufferTime;

            return true;
        }

        return false;
    }

    private IEnumerator RunBufferTimer(System.Action<float> m_bufferTimerRef, float p_bufferTime)
    {
        float t = 0;

        while (t < p_bufferTime)
        {
            t += Time.deltaTime;
            m_bufferTimerRef(t);
            yield return null;
        }

        m_bufferTimerRef(p_bufferTime);
    }

    #endregion

    #region Wall Run Code

    private void CheckWallRun()
    {
        float m_angleBetweenRays = m_wallRaySpacing / m_wallRidingRayCount;
        bool anyRayHit = false;

        for (int i = 0; i < m_wallRidingRayCount; i++)
        {
            Quaternion raySpaceQ = Quaternion.Euler(0, (i * m_angleBetweenRays) - (m_angleBetweenRays * (m_wallRidingRayCount / 2)), 0);
            RaycastHit hit;

            if (Physics.Raycast(m_characterController.transform.position, raySpaceQ * transform.forward, out hit, m_wallRunRayLength, m_wallMask))
            {
                if (Vector3.Dot(hit.normal, Vector3.up) == 0)
                {
                    anyRayHit = true;

                    m_wallVector = Vector3.Cross(hit.normal, Vector3.up);
                    m_wallFacingVector = Vector3.Cross(hit.normal, m_camera.transform.forward);
                    m_wallDir = hit.normal;

                    StartCoroutine(RunBufferTimer((x) => m_wallJumpBufferTimer = (x), m_wallJumpBufferTime));

                    CheckToStartWallRun();
                }

                Debug.DrawLine(m_characterController.transform.position, hit.point);
            }
        }

        if (!anyRayHit)
        {
            m_isWallRunning = false;
        }

    }

    private void TiltLerp()
    {
        m_cameraTilt.localRotation = Quaternion.Slerp(m_cameraTilt.localRotation, Quaternion.Euler(0, 0, m_tiltTarget), m_tiltSpeed);
    }

    private void OnWallRideRelease()
    {
        m_isWallRunning = false;

        StartCoroutine(RunBufferTimer((x) => m_wallRunBufferTimer = (x), m_wallRunBufferTime));
    }

    private void CheckToStartWallRun()
    {
        if (m_holdingWallRideStick)
        {
            if (!m_isWallRunning)
            {
                if (CheckOverBuffer(ref m_wallRunBufferTimer, ref m_wallRunBufferTime))
                {
                    StartCoroutine(WallRunning());
                    return;
                }
            }
        }

    }

    private IEnumerator WallRunning()
    {
        m_isWallRunning = true;

        m_states.m_gravityControllState = GravityState.GravityDisabled;
        m_states.m_movementControllState = MovementControllState.MovementDisabled;

        m_currentWallRunningSpeed = 0;

        float t = 0;

        while (m_isWallRunning)
        {
            t += Time.deltaTime;

            m_leapingTimer = 0;

            float result = Mathf.Lerp(-m_wallRunCameraMaxTilt, m_wallRunCameraMaxTilt, m_wallFacingVector.y);
            m_tiltTarget = result;

            m_velocity = (m_wallVector * -m_wallFacingVector.y) * m_currentMovementSpeed;
            m_velocity.y = 0;

            float progress = m_wallSpeedCurve.Evaluate(t / m_wallSpeedUpTime);
            m_currentWallRunningSpeed = Mathf.Lerp(0f, m_maxWallRunSpeed, progress);

            yield return null;
        }

        m_states.m_movementControllState = MovementControllState.MovementEnabled;
        m_states.m_gravityControllState = GravityState.GravityEnabled;

        m_currentWallRunningSpeed = 0;

        m_tiltTarget = 0f;
    }

    #endregion

    #region Jump Code
    public void OnJumpInputDown()
    {
        StartCoroutine(RunBufferTimer((x) => m_jumpBufferTimer = (x), m_jumpBufferTime));

        if (CheckBuffer(ref m_leapBufferTimer, ref m_leapBufferTime, IsGrounded()))
        {
            RunLeap();
            return;
        }

        if (CheckBuffer(ref m_wallJumpBufferTimer, ref m_wallJumpBufferTime, !m_isWallRunning))
        {
            WallJump();
            return;
        }

        if (CheckBuffer(ref m_graceTimer, ref m_graceTime, new bool[] { !IsGrounded(), m_velocity.y <= 0 }))
        {
            JumpMaxVelocity();
            return;
        }

        if (m_isWallRunning)
        {
            WallRunningJump();
            return;
        }

        if (IsGrounded())
        {
            JumpMaxVelocity();
            return;
        }

    }

    public void OnJumpInputUp()
    {
        /*
        if (m_velocity.y > m_minJumpVelocity)
        {
            JumpMinVelocity();
        }
        */
    }

    private void CalculateJump()
    {
        m_gravity = -(2 * m_maxJumpHeight) / Mathf.Pow(m_timeToJumpApex, 2);
        m_maxJumpVelocity = Mathf.Abs(m_gravity) * m_timeToJumpApex;
        m_minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(m_gravity) * m_minJumpHeight);
    }

    private void WallJump()
    {
        m_leapingTimer = 0;

        m_velocity.x = m_wallDir.x * m_wallJumpVelocity.x;
        m_velocity.y = m_wallJumpVelocity.y;
        m_velocity.z = m_wallDir.z * m_wallJumpVelocity.z;
    }

    private void WallRunningJump()
    {
        m_isWallRunning = false;

        StartCoroutine(RunBufferTimer((x) => m_wallRunBufferTimer = (x), m_wallRunBufferTime));

        m_leapingTimer = 0;

        m_velocity.x = m_wallDir.x * m_wallRunJumpVelocity.x;
        m_velocity.y = m_wallRunJumpVelocity.y;
        m_velocity.z = m_wallDir.z * m_wallRunJumpVelocity.z;
    }

    private void JumpMaxVelocity()
    {
        m_velocity.y = m_maxJumpVelocity;
    }

    private void JumpMinVelocity()
    {
        m_velocity.y = m_minJumpVelocity;
    }

    #endregion

    #region Leap Code

    private void RunLeap()
    {
        m_leapCount++;

        JumpMaxVelocity();

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
            m_currentLeapSpeed = Mathf.Lerp(0f, targetLeapSpeed, progress);

            yield return null;
        }

        m_currentLeapSpeed = 0;
        m_isLeaping = false;
    }

    #endregion

    #region Camera Code
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

    private void OnLanded()
    {
        m_isLanded = true;
        
        if (CheckBuffer(ref m_jumpBufferTimer, ref m_jumpBufferTime))
        {
            RunLeap();
        }

        StartCoroutine(RunBufferTimer((x) => m_leapBufferTimer = (x), m_leapBufferTime));
    }

    private void OnOffLedge()
    {
        m_offLedge = true;

        StartCoroutine(RunBufferTimer((x) => m_graceTimer = (x), m_graceTime));

    }
    #endregion

    #region Physics Calculation Code

    private void CalculateCurrentSpeed()
    {
        float speed = m_baseMovementSpeed;

        speed += m_currentLeapSpeed;
        speed += m_currentWallRunningSpeed;
        speed += m_currentSlideSpeed;

        m_currentMovementSpeed = speed;
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

        if (!IsGrounded() && !m_offLedge)
        {
            OnOffLedge();
        }
        if (IsGrounded())
        {
            m_offLedge = false;
        }

        if (IsGrounded() && !m_isLanded)
        {
            OnLanded();
        }
        if (!IsGrounded())
        {
            m_isLanded = false;
        }
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
    #endregion

    #region Slide Code

    private void StartSlide()
    {
        if (!m_isSliding)
        {
            if (IsGrounded())
            {
                StartCoroutine(Slide());
            }
        }
    }

    private IEnumerator Slide()
    {
        m_isSliding = true;

        float t = 0;

        while (t < m_slideTime)
        {
            t += Time.deltaTime;

            float progress = m_slideCurve.Evaluate(t / m_slideSpeedUpTime);
            m_currentSlideSpeed = Mathf.Lerp(0f, m_maxSlideSpeed, progress);

            m_leapingTimer = 0;

            yield return null;
        }

        m_currentSlideSpeed = 0;

        m_isSliding = false;
    }

    #endregion

    private void FillSpeedBar()
    {
        float progress = m_speedBarCurve.Evaluate(m_currentMovementSpeed / m_maxSpeed);

        m_speedometer.fillAmount = progress;

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



    private void OnCollisionEnter(Collision collision)
    {
        m_isStunned = false;
    }
}
