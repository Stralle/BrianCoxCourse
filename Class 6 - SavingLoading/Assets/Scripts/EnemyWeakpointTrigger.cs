using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeakpointTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            EnemyLogic enemyLogic = GetComponentInParent<EnemyLogic>();
            if (enemyLogic)
            {
                enemyLogic.SetState(EnemyState.Dead);
            }
        }
    }
}
