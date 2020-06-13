using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    // Serialized fields
    [SerializeField]
    float m_movementSpeed = 7.0f;
    [SerializeField]
    Transform m_weaponEquippedPosition;
    [SerializeField]
    Text m_healthText;
    [SerializeField]
    AudioClip m_damageTakenSound;
    [SerializeField]
    int m_playerHealth = 100;

    // Components
    CharacterController m_characterController;
    AudioSource m_audioSource;

    // Movement states
    float m_horizontalInput;
    float m_verticalInput;
    Vector3 m_movementInput;
    Vector3 m_movement;

    // Jumping
    float m_jumpHeight = 2.0f;
    float m_gravity = 0.05f;
    bool m_jump = false;

    // Interactive objects
    GameObject m_interactiveObject = null;
    GameObject m_equippedObject = null;

    void Start()
    {
        m_characterController = GetComponent<CharacterController>();
        m_audioSource = GetComponent<AudioSource>();
        SetHealthText();
        CheckForErrors();
    }

    void CheckForErrors()
    {
        if (!m_damageTakenSound || !m_healthText || !m_weaponEquippedPosition)
        {
            Debug.LogError("Check serialized fields in player!");
        }
    }

    void Update()
    {
        ProcessInput();
        CheckJump();
        InteractWithWeapon();
    }

    // Equipping and unequipping weapon
    private void InteractWithWeapon()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            if (m_equippedObject)
            {
                DropEquippedWeapon();
            }

            else if (m_interactiveObject) // Else because we don't want to immediately equip the dropped weapon.
            {
                GunLogic gunLogic = m_interactiveObject.GetComponent<GunLogic>();
                if (gunLogic)
                {
                    // Sets the position of the gun
                    m_interactiveObject.transform.position = m_weaponEquippedPosition.position;
                    m_interactiveObject.transform.rotation = m_weaponEquippedPosition.rotation;
                    m_interactiveObject.transform.parent = gameObject.transform;

                    // Deactivates gravity, deactivates collider
                    gunLogic.EquipGun();

                    m_equippedObject = m_interactiveObject;
                }
            }
        }
    }

    void DropEquippedWeapon()
    {
        GunLogic equippedGunLogic = m_equippedObject.GetComponent<GunLogic>();
        if (equippedGunLogic)
        {
            // Sets the position of the gun
            m_equippedObject.transform.parent = null;

            // Activates gravity, eactivates collider
            equippedGunLogic.UnequipGun();

            m_equippedObject = null;
        }
    }

    private void ProcessInput()
    {
        m_horizontalInput = Input.GetAxis("Horizontal");
        m_verticalInput = Input.GetAxis("Vertical");

        m_movementInput = new Vector3(m_horizontalInput, 0, m_verticalInput);
    }

    private void CheckJump()
    {
        if (!m_jump && Input.GetButtonDown("Jump") && m_movement.y == 0)
        {
            m_jump = true;
        }
    }

    void RotateCharacterTowardsMouseCoursor()
    {
        Vector3 mousePosInScreenSpace = Input.mousePosition;
        Vector3 playerPosInScreenSpace = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 directionInScreenSpace = mousePosInScreenSpace - playerPosInScreenSpace;

        float angle = Mathf.Atan2(directionInScreenSpace.y, directionInScreenSpace.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.AngleAxis(-angle + 180.0f, Vector3.up);
    }

    void FixedUpdate()
    {
        m_movement = m_movementInput * m_movementSpeed * Time.deltaTime;

        RotateCharacterTowardsMouseCoursor();
        ApplyGravity();
        Jump();

        if (m_characterController)
        {
            m_characterController.Move(m_movement);
        }
    }

    private void Jump()
    {
        if (m_jump)
        {
            m_movement.y = m_jumpHeight;
            m_jump = false;
        }
    }

    private void ApplyGravity()
    {
        if (!m_characterController.isGrounded)
        {
            if (m_movement.y > 0)
            {
                m_movement.y -= m_gravity;
            }
            else
            {
                m_movement.y -= m_gravity * 1.5f;
            }
        }
        else
        {
            m_movement.y = 0;
        }
    }

    private void SetHealthText()
    {
        m_healthText.text = "Health: " + m_playerHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Weapon")
        {
            m_interactiveObject = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Weapon" && m_interactiveObject == other.gameObject)
        {
            m_interactiveObject = null;
        }
    }

    public void TakeDamage(int damage)
    {
        if (m_audioSource && m_damageTakenSound)
        {
            m_audioSource.PlayOneShot(m_damageTakenSound);
        }
        if (m_playerHealth > 0)
        {
            m_playerHealth -= damage;
            SetHealthText();
        }
        if (m_playerHealth <= 0)
        {
            Die();
            m_playerHealth = 0;
            SetHealthText();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
        SceneManager.LoadScene(0);
    }
}
