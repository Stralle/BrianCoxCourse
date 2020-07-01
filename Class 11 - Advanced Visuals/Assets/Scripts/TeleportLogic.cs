using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportLogic : MonoBehaviour
{

    [SerializeField] private Transform m_transformPosition;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Teleport(other.gameObject);
        }
    }

    void Teleport(GameObject obj)
    {
        PlayerLogic playerLogic = (PlayerLogic) obj.GetComponent<PlayerLogic>();
        playerLogic.Teleport();
    }
}
