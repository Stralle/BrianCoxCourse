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
    private Animator m_animator;

    private bool m_jump = false;
    float m_jumpHeight = 0.25f;
    float m_gravity = 0.981f;

    Vector3 m_movement;
    Vector3 m_heightMovement;

    [SerializeField] private PlayerID m_playerID;

    private bool m_isCastingFireball = false;
    private bool m_isDead = false;

    [SerializeField] private Transform m_fireballSpawn;

    [SerializeField] private GameObject m_fireball;

    private const float MAX_RESPAWN_TIME = 2.0f;
    private float m_respawnTimer = MAX_RESPAWN_TIME;

    private Vector3 m_spawnPos;

    // Events
    public delegate void PlayerDeath(int playerNum);

    public static event PlayerDeath OnPlayerDeath;

    void Start()
    {
        m_characterController = GetComponent<CharacterController>();
        m_animator = GetComponent<Animator>();
        m_spawnPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_isDead)
        {
            return;
        }
        m_horizontalInput = Input.GetAxis("Horizontal" + m_playerID);
        m_verticalInput = Input.GetAxis("Vertical" + m_playerID);

        if (Input.GetButtonDown("Jump" + m_playerID) && m_characterController && m_characterController.isGrounded)
        {
            m_jump = true;
        }

        if (Input.GetButtonDown("Fire1" + m_playerID) && m_animator)
        {
            m_animator.SetTrigger("CastFireball");
            m_isCastingFireball = true;
        }
    }

    private void FixedUpdate()
    {
        if (m_isDead)
        {
            m_respawnTimer -= Time.deltaTime;
            if (m_respawnTimer <= 0)
            {
                Respawn();
            }

            return;
        }

        if (m_jump)
        {
            m_heightMovement.y = m_jumpHeight;
            m_jump = false;
        }

        m_heightMovement.y -= m_gravity * Time.deltaTime;
        
        m_movement = new Vector3(m_horizontalInput, 0, m_verticalInput) * m_movementSpeed * Time.deltaTime;

        if (m_animator)
        {
            // Abs should be done before maxing, because we want to get -1 chosen instead of 0.
            // So the input value would be 1 and the animation would be played also when we go backwards (s/a)
            m_animator.SetFloat("MovementInput", Mathf.Max(Mathf.Abs(m_horizontalInput), Mathf.Abs(m_verticalInput))); 
        }

        // Rotate towards movement direction
        if (m_movement != Vector3.zero) // Keep it facing the direction where he was previously moved. Do not reset it to 0.
        {
            transform.forward = m_movement.normalized; // Make a player face the direction where he's moving to
        }

        // We don't want to move when casting a fireball.
        if (m_isCastingFireball)
        {
            m_movement = Vector3.zero;
        }

        m_characterController.Move(m_heightMovement + m_movement);

        if (m_characterController.isGrounded)
        {
            m_heightMovement.y = 0;
        }
    }

    private void Respawn()
    {
        m_isDead = false;
        if (m_characterController)
        {
            m_characterController.enabled = false;
            transform.position = m_spawnPos;
            m_characterController.enabled = true;
        }

        m_respawnTimer = MAX_RESPAWN_TIME;

        if (m_animator)
        {
            m_animator.SetTrigger("Respawn");
        }
    }

    public void SetCastingFireballState(bool isCasting)
    {
        m_isCastingFireball = isCasting;
    }

    // This is an event added to the fireball animation
    public void ReleaseFireball()
    {
        // Spawn a fireball at specific point and in exact time.
        Instantiate(m_fireball, m_fireballSpawn.transform.position, transform.rotation);
    }

    public void Die()
    {
        m_isDead = true;
        if (m_animator)
        {
            m_animator.SetTrigger("Die");
        }
        if (m_characterController)
        {
            m_characterController.enabled = false;
        }

        if (OnPlayerDeath != null)
        {
            OnPlayerDeath(GetPlayerNum());
        }
    }

    int GetPlayerNum()
    {
        if (m_playerID == PlayerID._P1)
        {
            return 1;
        }
        else if (m_playerID == PlayerID._P2)
        {
            return 2;
        }

        return 0;
    }
}
