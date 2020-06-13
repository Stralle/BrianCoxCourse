using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


enum EPlayerState
{
    Idle = 0,
    Moving,
    AttackMoving,
    Count,
    Invalid = -1
}

public class RaycastLogic : MonoBehaviour
{
    // Serialized fields
    [SerializeField]
    GameObject m_waypointObject;
    [SerializeField]
    float m_meleeRange = 1.5f;

    // Components
    NavMeshAgent m_navMeshAgent;
    SwordLogic m_swordLogic;

    // States
    float m_waypointSize;
    EPlayerState m_playerState = EPlayerState.Invalid;
    GameObject m_enemyTarget;

    // Consts
    const float MAX_WAYPOINT_SIZE = 1.0f;
    const float MAX_CLICK_DISTANCE = 100.0f; 

    void Start()
    {
        m_navMeshAgent = GetComponent<NavMeshAgent>();
        m_swordLogic = GetComponentInChildren<SwordLogic>();
        m_playerState = EPlayerState.Idle;

        CheckComponentsAndFields();
    }

    void CheckComponentsAndFields()
    {
        if (!m_navMeshAgent)
        {
            Debug.LogError("NavMeshAgent is null");
        }
        if (!m_swordLogic)
        {
            Debug.LogError("SwordLogic is null");
        }
        if (!m_waypointObject)
        {
            Debug.LogError("Waypoint object is null!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            RaycastCameraToMouse();
        }

        ReduceWaypointSize();

        if (m_enemyTarget && m_playerState == EPlayerState.AttackMoving)
        {
            m_navMeshAgent.SetDestination(m_enemyTarget.transform.position);
            m_navMeshAgent.isStopped = false;
        }
        CheckAttackRange();
    }

    private void ReduceWaypointSize()
    {
        if (m_waypointObject && m_waypointSize > 0.0f)
        {
            m_waypointSize -= Time.deltaTime;
            m_waypointObject.transform.localScale = m_waypointSize * Vector3.one;
        }
    }

    void RaycastCameraToMouse()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, MAX_CLICK_DISTANCE))
        {
            Debug.Log("We hit an object: " + hit.collider.gameObject.name);
            Debug.Log("We hit an object at position: " + hit.point);

            m_navMeshAgent.SetDestination(hit.point);
            m_navMeshAgent.isStopped = false;
            
            if (hit.collider.gameObject.tag == "Enemy")
            {
                m_enemyTarget = hit.collider.gameObject;
                m_playerState = EPlayerState.AttackMoving;
            }
            else
            {
                m_enemyTarget = null;
                m_playerState = EPlayerState.Moving;
            }

            DisplayClickObject(hit.point);
        }
    }

    // Setting fullsize of a displayed object in 3d world
    private void DisplayClickObject(Vector3 pos)
    {
        if (!m_waypointObject)
        {
            return;
        }
        m_waypointSize = MAX_WAYPOINT_SIZE;
        m_waypointObject.transform.localScale = Vector3.one;
        m_waypointObject.transform.position = pos;
    }

    void CheckAttackRange() // Check if an enemy is in range of a melee attack.
    {
        Debug.DrawRay(transform.position, transform.forward * m_meleeRange, Color.red);

        if (!m_enemyTarget || m_playerState != EPlayerState.AttackMoving)
        {
            return;
        }

        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out hit, m_meleeRange))
        {
            if (hit.collider.gameObject.tag == "Enemy")
            {
                if (m_swordLogic)
                {
                    m_swordLogic.SetAttacking(true);
                }
                m_navMeshAgent.isStopped = true;
            }
            else
            {
                m_swordLogic.SetAttacking(false);
            }
        }
    }

    // Just a getter for a sword logic component
    public EnemyLogic GetTargetEnemyLogic()
    {
        if (m_enemyTarget)
        {
            EnemyLogic enemyLogic = m_enemyTarget.GetComponent<EnemyLogic>();
            if (enemyLogic)
            {
                return enemyLogic;
            }
        }
        return null;
    }
}
