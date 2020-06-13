using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogic : MonoBehaviour
{
    [SerializeField]
    int m_health = 100;

    public void TakeDamage(int damage)
    {
        m_health -= damage;

        if (m_health <= 0)
        {
            Destroy(gameObject);
        }
    }

}
