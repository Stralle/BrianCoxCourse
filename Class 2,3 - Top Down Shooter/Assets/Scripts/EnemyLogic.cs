using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*
 * Enum for enemy's states for AI / state-machine.
 */
enum EEnemyState
{
    Idle = 0,
    Patrol,
    Chase,
    Attack,
    Count,
    Invalid = -1
}

public class EnemyLogic : MonoBehaviour
{
    // Serialized fields
    [SerializeField]
    EEnemyState m_currentState = EEnemyState.Idle;
    [SerializeField]
    Transform m_destination;
    [SerializeField]
    Transform m_patrolStartPosition;
    [SerializeField]
    Transform m_patrolEndPosition;
    [SerializeField]
    int m_damageToDeal = 20;
    [SerializeField]
    int m_health = 100;
    [SerializeField]
    AudioClip m_damageTakenSound;
    [SerializeField]
    AudioClip m_enemyAttackSound;
    [SerializeField]
    AudioClip m_DeathSound;
    [SerializeField]
    GameObject m_player;
    [SerializeField]
    float m_catchingRadius = 5.0f;
    [SerializeField]
    float m_meleeRadius = 2.0f;
    [SerializeField]
    float m_stoppingDistance = 1.5f;
    [SerializeField]
    float m_attackCooldown = MAX_ATTACK_COOLDOWN;

    // Components
    NavMeshAgent m_navMeshAgent;
    AudioSource m_audioSource;

    // States
    Vector3 m_currentPatrolDestination;

    // Consts
    const float MAX_ATTACK_COOLDOWN = 0.5f;

    void Start()
    {
        m_navMeshAgent = GetComponent<NavMeshAgent>();
        m_currentPatrolDestination = m_patrolStartPosition.position;
        m_audioSource = GetComponent<AudioSource>();

        m_player = GameObject.FindGameObjectWithTag("Player");
    }

    // For debugging purposes. Drawing in 3D world a sphere to see what is the catching radius for an enemy
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.25f);
        Gizmos.DrawSphere(transform.position, m_catchingRadius);
    }

    void Update()
    {
        switch (m_currentState)
        {
            case (EEnemyState.Idle):
                SearchForPlayer();
                break;
            case (EEnemyState.Patrol):
                Patrol();
                break;
            case (EEnemyState.Chase):
                ChasePlayer();
                break;
            case (EEnemyState.Attack):
                AttackPlayer();
                break;
            default:
                // Do nothing
                break;
        }
    }

    private void AttackPlayer()
    {
        if (!m_player)
        {
            return;
        }

        // Finding a distance between a player and an enemy
        float distance = Vector3.Distance(transform.position, m_player.transform.position);

        if (distance < m_meleeRadius)
        {
            m_attackCooldown -= Time.deltaTime; // Reducing cooldown time for attack
            if (m_attackCooldown <= 0.0f)
            {
                Player playerLogic = m_player.GetComponent<Player>();
                if (playerLogic)
                {
                    playerLogic.TakeDamage(m_damageToDeal);
                }
                PlaySound(m_enemyAttackSound);
                m_attackCooldown = MAX_ATTACK_COOLDOWN;
            }
        }
        else
        {
            m_currentState = EEnemyState.Chase;
        }
    }

    private void ChasePlayer()
    {
        if (!m_player || m_currentState != EEnemyState.Chase || !m_destination)
        {
            return;
        }

        if (m_navMeshAgent)
        {
            m_navMeshAgent.SetDestination(m_destination.position);
        }

        float distance = Vector3.Distance(m_destination.position, transform.position);
        if (distance < m_stoppingDistance)
        {
            m_navMeshAgent.isStopped = true;
            m_navMeshAgent.velocity = Vector3.zero;

            m_currentState = EEnemyState.Attack;
        }
        else
        {
            m_navMeshAgent.isStopped = false;
        }
    }

    private void SearchForPlayer()
    {
        if (!m_player)
        {
            return;
        }

        float distance = Vector3.Distance(transform.position, m_player.transform.position);
        
        if (distance < m_catchingRadius)
        {
            m_currentState = EEnemyState.Chase;
        }
    }

    private void Patrol()
    {
        if (m_currentState != EEnemyState.Patrol || !m_patrolStartPosition || !m_patrolEndPosition)
        {
            return;
        }

        if (m_navMeshAgent && m_currentPatrolDestination != Vector3.zero)
        {
            m_navMeshAgent.SetDestination(m_currentPatrolDestination);
        }

        float distance = Vector3.Distance(m_currentPatrolDestination, transform.position);
        if (distance < m_stoppingDistance)
        {
            if (m_currentPatrolDestination == m_patrolStartPosition.position)
            {
                m_currentPatrolDestination = m_patrolEndPosition.position;
            }
            else
            {
                m_currentPatrolDestination = m_patrolStartPosition.position;
            }
        }
    }

    // Receiving a damage from a bullet
    public void TakeDamage(int damage)
    {
        m_health -= damage;

        if (m_health > 0)
        {
            PlaySound(m_damageTakenSound);
        }
        else
        {
            PlaySound(m_DeathSound);
            m_health = 0;
            m_currentState = EEnemyState.Invalid;
            Destroy(gameObject, 1.5f);              // Destroying game object 1.5 seconds after to make a sound play a bit longer
        }
    }

    private void PlaySound(AudioClip soundToPlay)
    {
        if (m_audioSource && soundToPlay)
        {
            m_audioSource.PlayOneShot(soundToPlay);
        }
    }
}
