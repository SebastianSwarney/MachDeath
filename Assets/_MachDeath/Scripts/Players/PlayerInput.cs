using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerInput : MonoBehaviour
{
    public int m_playerId;

    private PlayerMovementController m_playerMovementController;
    private Player m_playerInputController;

    private bool m_doTheLookyLook;

    private void Start()
    {
        m_playerMovementController = GetComponent<PlayerMovementController>();
        m_playerInputController = ReInput.players.GetPlayer(m_playerId);
    }

    private void Update()
    {
        GetInput();
    }

    public void GetInput()
    {
        
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
    }
}
