using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    float m_horizontalInput;
    float m_movementSpeed = 5.0f;

    CharacterController m_characterController;
    Animator m_animator;

    bool m_jump;
    float m_jumpHeight = 0.25f;
    float m_gravity = 0.981f;

    Vector3 m_horizontalMovement;
    Vector3 m_heightMovement;

    void Start()
    {
        m_characterController = GetComponent<CharacterController>();
        m_animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        m_horizontalInput = Input.GetAxis("Horizontal");
        
        if (Input.GetButtonDown("Jump"))
        {
            m_jump = true;
        }

        if (m_animator)
        {
            m_animator.SetFloat("MovementInput", Mathf.Abs(m_horizontalInput)); // Absolute so it could work for left as well
        }
    }

    void FixedUpdate()
    {
        UpdateRotation();

        if (m_jump)
        {
            m_heightMovement.y = m_jumpHeight;
            m_jump = false;
        }

        m_heightMovement.y -= m_gravity * Time.deltaTime; // Simulating a gravity
        m_horizontalMovement = Vector3.right * m_horizontalInput * m_movementSpeed * Time.deltaTime; // side-scrolling moving

        if(m_characterController)
        {
            m_characterController.Move(m_horizontalMovement + m_heightMovement);
        }

        if (m_characterController.isGrounded)
        {
            m_heightMovement.y = 0.0f;
        }
    }

    void UpdateRotation()
    {
        if (m_horizontalInput > 0.0f)
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        else if (m_horizontalInput < 0.0f)
        {
            transform.rotation = Quaternion.Euler(0, -90, 0);
        }
    }

    public void Save()
    {
        PlayerPrefs.SetFloat("PositionX", transform.position.x);
        PlayerPrefs.SetFloat("PositionY", transform.position.y);
        PlayerPrefs.SetFloat("PositionZ", transform.position.z);

        PlayerPrefs.SetFloat("RotationX", transform.rotation.eulerAngles.x); // we have to save it as eulerAngles, otherwise we'll get quaternion values.
        PlayerPrefs.SetFloat("RotationY", transform.rotation.eulerAngles.y);
        PlayerPrefs.SetFloat("RotationZ", transform.rotation.eulerAngles.z);
    }

    public void Load()
    {
        float positionX = PlayerPrefs.GetFloat("PositionX");
        float positionY = PlayerPrefs.GetFloat("PositionY");
        float positionZ = PlayerPrefs.GetFloat("PositionZ");

        float RotationX = PlayerPrefs.GetFloat("RotationX");
        float RotationY = PlayerPrefs.GetFloat("RotationY");
        float RotationZ = PlayerPrefs.GetFloat("RotationZ");

        m_characterController.enabled = false;
        transform.position = new Vector3(positionX, positionY, positionZ);
        transform.rotation = Quaternion.Euler(RotationX, RotationY, RotationZ);
        m_characterController.enabled = true;
    }
}
