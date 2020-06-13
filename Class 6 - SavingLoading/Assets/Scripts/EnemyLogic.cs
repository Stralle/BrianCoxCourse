using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

public enum EnemyState
{
    Idle,
    StandingUp,
    Attack,
    Dead
}

public class EnemyLogic : MonoBehaviour
{
    private GameObject m_player;

    private Animator m_animator;

    private CharacterController m_characterController;

    private float m_aggroRadius = 4.0f;

    private EnemyState m_enemyState = EnemyState.Idle;

    private float m_movementSpeed = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        m_player = GameObject.FindGameObjectWithTag("Player");
        m_animator = GetComponent<Animator>();
        m_characterController = GetComponent<CharacterController>();
        m_characterController.enabled = false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1,0,0, 0.25f);
        Gizmos.DrawSphere(transform.position, m_aggroRadius);
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_player)
        {
            return;
        }
        float distance = Vector3.Distance(m_player.transform.position, transform.position);
        if (distance < m_aggroRadius && m_animator && m_enemyState == EnemyState.Idle)
        {
            m_enemyState = EnemyState.StandingUp;
            m_animator.SetTrigger("PlayerNearby");
        }
    }

    private void FixedUpdate()
    {
        if (m_enemyState == EnemyState.Attack && m_characterController)
        {
            m_characterController.Move(transform.forward * m_movementSpeed * Time.deltaTime);
        }
    }

    public void SetState(EnemyState enemyState)
    {
        m_enemyState = enemyState;

        if (m_enemyState == EnemyState.Attack)
        {
            m_characterController.enabled = true;
        }
        else if (m_enemyState == EnemyState.Dead && m_animator)
        {
            m_animator.SetBool("IsDead", true);
            m_characterController.enabled = false;

        }
    }

    public void Save(int index)
    {
        PlayerPrefs.SetInt("EnemyState" + index, (int)m_enemyState); // We have to have unique identifier for each coin.

        PlayerPrefs.SetFloat("EnemyPosX" + index, transform.position.x);
        PlayerPrefs.SetFloat("EnemyPosY" + index, transform.position.y);
        PlayerPrefs.SetFloat("EnemyPosZ" + index, transform.position.z);

        AnimatorStateInfo info = m_animator.GetCurrentAnimatorStateInfo(0);
        int animHash = info.fullPathHash;
        PlayerPrefs.SetInt("EnemyAnimHash" + index, animHash);
        float animTime = info.normalizedTime;
        PlayerPrefs.SetFloat("EnemyAnimTime" + index, animTime);

        // If we add enemies going to the different directions then we'd need rotation to save as well.
    }

    public void Load(int index)
    {
        m_enemyState = (EnemyState)PlayerPrefs.GetInt("EnemyState" + index);

        float enemyPosX = PlayerPrefs.GetFloat("EnemyPosX" + index);
        float enemyPosY = PlayerPrefs.GetFloat("EnemyPosY" + index);
        float enemyPosZ = PlayerPrefs.GetFloat("EnemyPosZ" + index);

        m_characterController.enabled = false;
        transform.position = new Vector3(enemyPosX, enemyPosY, enemyPosZ);
        m_characterController.enabled = m_enemyState == EnemyState.Attack;

        m_animator.SetBool("IsDead", m_enemyState == EnemyState.Dead);

        int animHash = PlayerPrefs.GetInt("EnemyAnimHash" + index);
        float animTime = PlayerPrefs.GetFloat("EnemyAnimTime" + index);

        m_animator.Play(animHash, 0, animTime);
    }
}
