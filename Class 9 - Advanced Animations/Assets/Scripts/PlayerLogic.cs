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
    [SerializeField]
    Transform m_leftFoot;
    [SerializeField]
    Transform m_rightFoot;
    [SerializeField]
    Transform m_leftHand;

    // States
    float m_horizontalInput;
    float m_verticalInput;
    Vector3 m_heightMovement;
    Vector3 m_verticalMovement;
    Vector3 m_horizontalMovement;

    bool m_jump = false;
    private bool m_isCrouching = false;

    private float m_raycastHeightOffset = 0.5f;
    private float m_raycastPositioningOffset = 0.15f;

    // Components
    CharacterController m_characterController;
    Animator m_animator;
    GameObject m_camera;
    CameraLogic m_cameraLogic;
    private WeaponLogic m_weaponLogic;

    void Start()
    {
        m_characterController = GetComponent<CharacterController>();
        m_animator = GetComponent<Animator>();
        m_camera = Camera.main.gameObject;
        if (m_camera)
        {
            m_cameraLogic = m_camera.GetComponent<CameraLogic>();
        }

        m_weaponLogic = GetComponentInChildren<WeaponLogic>();

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
        if (Input.GetKeyDown(KeyCode.C) && m_characterController.isGrounded)
        {
            m_isCrouching = !m_isCrouching;
            if (m_animator)
            {
                m_animator.SetBool("IsCrouching", m_isCrouching);
            }
        }

        if (m_isCrouching)
        {
            m_horizontalInput = 0;
            m_verticalInput = 0;
            return;
        }

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
        
        if (m_characterController.isGrounded)
        {
            m_heightMovement.y = 0.0f;
        }
    }

    private void ApplyGravity()
    {
        m_heightMovement.y -= m_gravity * Time.deltaTime;
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (m_animator && m_cameraLogic)
        {
            m_animator.SetBoneLocalRotation(HumanBodyBones.Neck, Quaternion.Euler(m_cameraLogic.GetRotationX(), 0, 0));
            m_animator.SetBoneLocalRotation(HumanBodyBones.RightShoulder, Quaternion.Euler(m_cameraLogic.GetRotationX(), 0, 0));
            m_animator.SetBoneLocalRotation(HumanBodyBones.LeftShoulder, Quaternion.Euler(m_cameraLogic.GetRotationX(), 0, 0));
        }

        if (m_leftHand && m_animator)
        {
            if (!m_weaponLogic.IsReloading())
            {
                m_animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
                m_animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1); // 1 is maximum value
                m_animator.SetIKPosition(AvatarIKGoal.LeftHand, m_leftHand.position);
                m_animator.SetIKRotation(AvatarIKGoal.LeftHand, m_leftHand.rotation);
            }
            else
            {
                m_animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
                m_animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
            }
        }

        if (m_leftFoot)
        {
            UpdateFootIK(AvatarIKGoal.LeftFoot, m_leftFoot);
            UpdateFootIK(AvatarIKGoal.RightFoot, m_rightFoot);
        }
    }

    // For making only a foot stays up on the obstacle.
    // Lower character controller radius to 0.1 or something like that to make it work!
    void UpdateFootIK(AvatarIKGoal avatarIKGoal, Transform footTransform)
    {
        Vector3 rayStartPosition = footTransform.position;
        rayStartPosition.y += m_raycastHeightOffset; // raycast position and until where we want to go.

        Ray ray = new Ray(rayStartPosition, Vector3.down);
        RaycastHit hit;
        LayerMask obstacleLayerMask = LayerMask.GetMask("Obstacle");

        if (Physics.Raycast(ray, out hit, 1.0f, obstacleLayerMask)) // to hit only obstacles
        {
            Vector3 targetPosition = hit.point;
            targetPosition.y += m_raycastPositioningOffset; // to not put foot on exact raycast hit point but to a slightly higher position.

            m_animator.SetIKPositionWeight(avatarIKGoal, 1);
            m_animator.SetIKRotationWeight(avatarIKGoal, 1); // 1 is maximum value
            m_animator.SetIKPosition(avatarIKGoal, targetPosition);
            m_animator.SetIKRotation(avatarIKGoal, footTransform.rotation);
        }
    }
}
