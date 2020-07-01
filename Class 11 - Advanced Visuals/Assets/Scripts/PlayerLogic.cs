using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    // Serialized fields
    [SerializeField]
    float m_movementSpeed = 5.0f;
    [SerializeField]
    float m_jumpHeight = 0.4f;
    [SerializeField]
    float m_gravity = 1.25f;

    // States
    float m_horizontalInput;
    float m_verticalInput;
    Vector3 m_heightMovement;
    Vector3 m_verticalMovement;
    Vector3 m_horizontalMovement;

    bool m_jump = false;

    // Components
    CharacterController m_characterController;
    Animator m_animator;
    AudioSource m_audioSource;
    GameObject m_camera;
    CameraLogic m_cameraLogic;

    private RenderTexCamLogic m_renderTexCamLogic;

    void Start()
    {
        m_characterController = GetComponent<CharacterController>();
        m_animator = GetComponent<Animator>();
        m_audioSource = GetComponent<AudioSource>();
        m_camera = Camera.main.gameObject;
        m_renderTexCamLogic = FindObjectOfType<RenderTexCamLogic>();

        if (m_camera)
        {
            m_cameraLogic = m_camera.GetComponent<CameraLogic>();
        }

        CheckComponentsAndFields();
    }

    private void CheckComponentsAndFields()
    {
        if (!m_characterController)
        {
            Debug.LogError("CharacterController is null!");
        }
        if (!m_animator)
        {
            Debug.LogError("No animator!");
        }
        if (!m_camera)
        {
            Debug.LogError("No Camera!");
        }
        if (!m_cameraLogic)
        {
            Debug.LogError("No CameraLogic!");
        }
    }

    void Update()
    {
        m_horizontalInput = Input.GetAxis("Horizontal");
        m_verticalInput = Input.GetAxis("Vertical");

        if (Input.GetButtonDown("Jump") && m_characterController.isGrounded)
        {
            m_jump = true;
        }

        if (m_animator)
        {
            m_animator.SetFloat("HorizontalInput", m_horizontalInput);
            m_animator.SetFloat("VerticalInput", m_verticalInput);
        }
    }

    void FixedUpdate()
    {
        Jump();
        if (m_cameraLogic && (Mathf.Abs(m_horizontalInput) > 0.1f || Mathf.Abs(m_verticalInput) > 0.1f))
        {
            transform.forward = m_cameraLogic.GetForwardVector();
        }
        Move();
        ApplyGravity();
    }

    private void Jump()
    {
        if (m_jump)
        {
            m_heightMovement.y = m_jumpHeight;
            m_jump = false;
        }
    }

    private void Move()
    {
        m_verticalMovement = transform.forward * m_verticalInput * m_movementSpeed * Time.deltaTime;
        m_horizontalMovement = transform.right * m_horizontalInput * m_movementSpeed * Time.deltaTime;

        m_characterController.Move(m_horizontalMovement + m_verticalMovement + m_heightMovement);
        m_renderTexCamLogic.Move(m_horizontalMovement + m_verticalMovement + m_heightMovement);


        if (m_characterController.isGrounded)
        {
            m_heightMovement.y = 0.0f;
        }
    }

    private void ApplyGravity()
    {
        m_heightMovement.y -= m_gravity * Time.deltaTime;
    }

    void PlayRandomSound(List<AudioClip> audioClips)
    {
        if (m_audioSource && audioClips.Count > 0)
        {
            int randomFootstepSound = Random.Range(0, audioClips.Count); // Max exclusive
            m_audioSource.PlayOneShot(audioClips[randomFootstepSound]);
        }
    }

    public void Teleport()
    {
        if (m_characterController && m_renderTexCamLogic)
        {
            m_characterController.enabled = false;

            transform.position = m_renderTexCamLogic.transform.position;

            m_characterController.enabled = true;
        }
    }
}
