using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordLogic : MonoBehaviour
{
    // Serialized fields
    [SerializeField]
    int m_damageToDeal = 35;

    // Components
    Animator m_animator;
    RaycastLogic m_raycastLogic;

    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_raycastLogic = GetComponentInParent<RaycastLogic>();

        CheckComponents();
    }

    void CheckComponents()
    {
        if (!m_animator)
        {
            Debug.LogError("Animator is null!");
        }
        if (!m_raycastLogic)
        {
            Debug.LogError("RaycastLogic is null!");
        }
    }

    void Update()
    {
        UpdateAnimations();
    }

    void UpdateAnimations()
    {
        SetAttacking(Input.GetButton("Fire1"));
    }

    // Event called on weapon hit animation
    void WeaponHit()
    {
        Debug.Log("Weapon Hit! ");
        EnemyLogic enemyLogic = m_raycastLogic.GetTargetEnemyLogic();

        if (enemyLogic)
        {
            enemyLogic.TakeDamage(m_damageToDeal);
        }
    }

    public void SetAttacking(bool isAttacking)
    {
        m_animator.SetBool("IsAttacking", isAttacking);
    }
}
