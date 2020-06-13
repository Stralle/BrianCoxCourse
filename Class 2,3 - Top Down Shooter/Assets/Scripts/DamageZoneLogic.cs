using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Logic for a trap-like damage zone. Deals damage to players.
 * 
 */
public class DamageZoneLogic : MonoBehaviour
{
    [SerializeField]
    int m_damageToDeal = 20;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Player playerLogic = other.GetComponent<Player>();
            if (playerLogic)
            {
                playerLogic.TakeDamage(m_damageToDeal);
            }
        }
    }

}
