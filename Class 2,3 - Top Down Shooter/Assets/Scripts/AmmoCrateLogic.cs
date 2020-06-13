using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Logic for amoo crate
 * 
 */
public class AmmoCrateLogic : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") // Only on colision with player
        {
            GunLogic gunLogic = other.GetComponentInChildren<GunLogic>(); // Finding component GunLogic in the children of a Player game object.

            if (gunLogic)
            {
                gunLogic.RefillAmmo();
                Destroy(gameObject);
            }
        }
    }
}
