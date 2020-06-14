using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

public enum PlayerID
{
    _P1, _P2
}
public class PlayerLogic : MonoBehaviour
{
    float m_horizontalInput;
    float m_verticalInput;

    float m_movementSpeed = 5;

    CharacterController m_characterController;

    private bool m_jump = false;
    float m_jumpHeight = 0.25f;
    float m_gravity = 0.981f;

    Vector3 m_movement;
    Vector3 m_heightMovement;

    [SerializeField] private PlayerID m_playerID;
    // Start is called before the first frame update
    void Start()
    {
        m_characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        m_horizontalInput = Input.GetAxis("Horizontal" + m_playerID);
        m_verticalInput = Input.GetAxis("Vertical" + m_playerID);

        if (Input.GetButtonDown("Jump" + m_playerID) && m_characterController && m_characterController.isGrounded)
        {
            m_jump = true;
        }
    }

    private void FixedUpdate()
    {
        if (m_jump)
        {
            m_heightMovement.y = m_jumpHeight;
            m_jump = false;
        }

        m_heightMovement.y -= m_gravity * Time.deltaTime;

        m_movement = new Vector3(m_horizontalInput, 0, m_verticalInput) * m_movementSpeed * Time.deltaTime;
        m_characterController.Move(m_heightMovement + m_movement);

        if (m_characterController.isGrounded)
        {
            m_heightMovement.y = 0;
        }
    }
}
