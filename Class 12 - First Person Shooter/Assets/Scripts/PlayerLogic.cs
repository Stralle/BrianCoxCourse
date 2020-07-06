using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using Quaternion = UnityEngine.Quaternion;

public class PlayerLogic : MonoBehaviour
{
    private float m_rotationY;

    private float m_horizontalInput;
    private float m_verticalInput;

    private Vector3 m_horizontalMovement;
    private Vector3 m_verticalMovement;

    private const float K_ROTATION_SPEED = 2.0f;
    private const float K_MOVEMENT_SPEED = 5.0f;

    private CharacterController m_characterController;
    private Animator m_animator;


    void Start()
    {
        m_characterController = GetComponent<CharacterController>();
        m_animator = GetComponent<Animator>();
    }

    void Update()
    {
        ApplyCameraRotation();
        GetInput();
    }

    void FixedUpdate()
    {
        MovePlayer();
        SetAnimatorValues();
    }

    void ApplyCameraRotation()
    {
        m_rotationY += Input.GetAxis("Mouse X") * K_ROTATION_SPEED;

        transform.rotation = Quaternion.Euler(0, m_rotationY, 0);
    }

    void GetInput()
    {
        m_horizontalInput = Input.GetAxis("Horizontal");
        m_verticalInput = Input.GetAxis("Vertical");
    }

    void MovePlayer()
    {
        // These next two lines are used because if we just use movementInput combined of m_verticalInput and m_HorizontalInput
        // it will result going always forward/backward and left/right absolute to the world's coordinates.
        // And we want player to move relative to his position and rotation in all four directions.
        m_horizontalMovement = transform.right * m_horizontalInput * K_MOVEMENT_SPEED * Time.deltaTime;
        m_verticalMovement = transform.forward * m_verticalInput * K_MOVEMENT_SPEED * Time.deltaTime;

        if (m_characterController)
        {
            m_characterController.Move(m_horizontalMovement + m_verticalMovement);
        }
    }
    private void SetAnimatorValues()
    {
        if (m_animator)
        {
            m_animator.SetFloat("HorizontalInput", m_horizontalInput);
            m_animator.SetFloat("VerticalInput", m_verticalInput);
        }
    }

    public float GetRotationY()
    {
        return m_rotationY;
    }
}
