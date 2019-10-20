using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerInput : MonoBehaviour
{
    public int m_playerId;

    private PlayerMovementController m_playerMovementController;
    private Player m_playerInputController;

    private void Start()
    {
        m_playerMovementController = GetComponent<PlayerMovementController>();
        m_playerInputController = ReInput.players.GetPlayer(m_playerId);
    }

    private void Update()
    {
        Vector2 movementInput = new Vector2(m_playerInputController.GetAxis("MoveHorizontal"), m_playerInputController.GetAxis("MoveVertical"));
        m_playerMovementController.SetMovementInput(movementInput);

        Vector2 lookInput = new Vector2(m_playerInputController.GetAxis("LookHorizontal"), m_playerInputController.GetAxis("LookVertical"));
        m_playerMovementController.SetLookInput(lookInput);

        if (m_playerInputController.GetButtonDown("Jump"))
        {
            m_playerMovementController.OnJumpInputDown();
        }

        if (m_playerInputController.GetButtonUp("Jump"))
        {
            m_playerMovementController.OnJumpInputUp();
        }
    }
}
