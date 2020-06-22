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
    List<AudioClip> m_footstepStoneSounds = new List<AudioClip>();
    [SerializeField]
    List<AudioClip> m_footstepEarthSounds = new List<AudioClip>();
    [SerializeField]
    List<AudioClip> m_footstepGrassSounds = new List<AudioClip>();
    [SerializeField]
    List<AudioClip> m_footstepPuddleSounds = new List<AudioClip>();
    [SerializeField]
    Transform m_leftFoot;
    [SerializeField]
    Transform m_rightFoot;

    // States
    Vector3 m_movementInput;
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

    void Start()
    {
        m_characterController = GetComponent<CharacterController>();
        m_animator = GetComponent<Animator>();
        m_audioSource = GetComponent<AudioSource>();
        m_camera = Camera.main.gameObject;
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
        if (!m_audioSource)
        {
            Debug.LogError("No AudioSource!");
        }
        if (!m_camera)
        {
            Debug.LogError("No Camera!");
        }
        if (!m_cameraLogic)
        {
            Debug.LogError("No CameraLogic!");
        }
        if (m_footstepEarthSounds.Count <= 0)
        {
            Debug.LogError("Check footstep earth sounds!");
        }
        if (m_footstepStoneSounds.Count <= 0)
        {
            Debug.LogError("Check footstep stone sounds!");
        }
        if (m_footstepGrassSounds.Count <= 0)
        {
            Debug.LogError("Check footstep grass sounds!");
        }
        if (m_footstepPuddleSounds.Count <= 0)
        {
            Debug.LogError("Check footstep puddle sounds!");
        }
    }

    void Update()
    {
        m_horizontalInput = Input.GetAxis("Horizontal");
        m_verticalInput = Input.GetAxis("Vertical");

        m_movementInput = new Vector3(m_horizontalInput, 0, m_verticalInput);

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

    public void PlayFootstepSound (int footIndex)
    {
        // 0 = left, 1 = right;

        if (footIndex == 0)
        {
            RayCastTerrain(m_leftFoot.position);
        }
        else if (footIndex == 1)
        {
            RayCastTerrain(m_rightFoot.position);
        }
    }

    void RayCastTerrain(Vector3 position)
    {
        LayerMask layerMask = LayerMask.GetMask("Terrain");
        Ray ray = new Ray(position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, layerMask))
        {
            string hitTag = hit.collider.gameObject.tag;
            if (hitTag == "Earth")
            {
                PlayRandomSound(m_footstepEarthSounds);
            }
            else if (hitTag == "Stone")
            {
                PlayRandomSound(m_footstepStoneSounds);
            }
            else if (hitTag == "Grass")
            {
                PlayRandomSound(m_footstepGrassSounds);
            }
            else if (hitTag == "Puddle")
            {
                PlayRandomSound(m_footstepPuddleSounds);
            }
        }
    }

    void PlayRandomSound(List<AudioClip> audioClips)
    {
        if (m_audioSource && audioClips.Count > 0)
        {
            int randomFootstepSound = Random.Range(0, audioClips.Count); // Max exclusive
            m_audioSource.PlayOneShot(audioClips[randomFootstepSound]);
        }
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (m_animator && m_cameraLogic)
        {
            m_animator.SetBoneLocalRotation(HumanBodyBones.Neck, Quaternion.Euler(m_cameraLogic.GetRotationX(), 0, 0));
            m_animator.SetBoneLocalRotation(HumanBodyBones.RightShoulder, Quaternion.Euler(m_cameraLogic.GetRotationX(), 0, 0));
            m_animator.SetBoneLocalRotation(HumanBodyBones.LeftShoulder, Quaternion.Euler(m_cameraLogic.GetRotationX(), 0, 0));
        }
    }
}
