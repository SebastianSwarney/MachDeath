using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using Photon.Pun;

[System.Serializable]
public class PlayerInputEvent : UnityEngine.Events.UnityEvent { }
public class PlayerInput : MonoBehaviour
{


    public int m_playerId;
    private PlayerMovementController m_playerMovementController;
    private Player m_playerInputController;


    private bool m_doTheLookyLook;

    private WeaponHolder m_weaponHolder;
    [Header("Photon")]
    public GameObject m_camera;
    private PhotonView m_photonView;

    public PlayerInputEvent m_sprayEvent;
    public KeyCode m_sprayKeyCode;
    public KeyCode m_toggleInput;
    private bool m_hasInput = true;

    private void Start()
    {

        Cursor.lockState = CursorLockMode.Locked;
        m_playerMovementController = GetComponent<PlayerMovementController>();
        m_playerInputController = ReInput.players.GetPlayer(m_playerId);
        m_photonView = GetComponent<PhotonView>();
        m_weaponHolder = GetComponent<WeaponHolder>();
        if (m_photonView.IsMine)
        {
            m_camera.SetActive(true);
        }
    }

    private void Update()
    {
        if (!m_photonView.IsMine) return;
        GetInput();
        m_playerMovementController.PerformController();

    }

    public void GetInput()
    {

        if (Input.GetKeyDown(m_sprayKeyCode))
        {
            m_sprayEvent.Invoke();
        }
        if (Input.GetKeyDown(m_toggleInput))
        {
            m_hasInput = !m_hasInput;
            Cursor.lockState = (m_hasInput) ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !m_hasInput;
        }

        if (!m_hasInput) return;

        Vector2 movementInput = new Vector2(m_playerInputController.GetAxis("MoveHorizontal"), m_playerInputController.GetAxis("MoveVertical"));

        m_playerMovementController.SetMovementInput(movementInput);

        if (Input.GetKeyDown(KeyCode.P))
        {
            m_doTheLookyLook = !m_doTheLookyLook;
        }

        if (!m_doTheLookyLook)
        {
            Vector2 lookInput = new Vector2(m_playerInputController.GetAxis("LookHorizontal"), m_playerInputController.GetAxis("LookVertical"));
            m_playerMovementController.SetLookInput(lookInput);
        }



        if (m_playerInputController.GetButtonDown("Jump"))
        {
            m_playerMovementController.OnJumpInputDown();
        }

        if (m_playerInputController.GetButtonUp("Jump"))
        {
            m_playerMovementController.OnJumpInputUp();
        }

        if (m_playerInputController.GetButtonDown("WallRide"))
        {
            m_playerMovementController.WallRideInputDown();
        }

        if (m_playerInputController.GetButtonUp("WallRide"))
        {
            m_playerMovementController.WallRideInputUp();
        }

        if (m_playerInputController.GetButton("Fire"))
        {
            m_weaponHolder.ShootGun();
        }


    }
}
