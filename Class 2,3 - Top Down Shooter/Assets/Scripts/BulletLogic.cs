using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLogic : MonoBehaviour
{
    // Serialized fields
    [SerializeField]
    float m_bulletSpeed = 8.0f;
    [SerializeField]
    int m_damageToDeal = 20;
    [SerializeField]
    float m_bulletLifeTime = 2.0f;

    // Components
    Rigidbody m_rigidBody;

    void Start()
    {
        m_rigidBody = GetComponent<Rigidbody>();
        if (m_rigidBody)
        {
            m_rigidBody.velocity = transform.up * m_bulletSpeed; // Bullet has a constant speed defined on the start. Up because of orientation
        }
    }

    void FixedUpdate()
    {
        CheckBulletLifetime();
    }

    // Checking if the bullet should be destroyed due to time limit
    private void CheckBulletLifetime()
    {
        m_bulletLifeTime -= Time.fixedDeltaTime;
        if (m_bulletLifeTime <= 0)
        {
            DestroyBullet();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Target")
        {
            Destroy(other.gameObject);  // Destroying target
            DestroyBullet();            
        }
        else if (other.tag == "Enemy")
        {
            EnemyLogic enemyLogic = other.GetComponentInParent<EnemyLogic>();
            if (enemyLogic)
            {
                enemyLogic.TakeDamage(m_damageToDeal);
            }
            DestroyBullet();
        }
    }

    // Method where we want to destroy our bullet and parent object if there's any
    private void DestroyBullet()
    {
        if (transform.parent != null)
        {
            Destroy(transform.parent.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
