using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class TeleportLogic : MonoBehaviour
{

    [SerializeField] private Material m_daySkybox;
    private Camera m_mainCamera;
    private PostProcessingBehaviour m_postProcessingBehaviour;
    private void Start()
    {
        m_mainCamera = Camera.main;
        if (m_mainCamera)
        {
            m_postProcessingBehaviour = m_mainCamera.GetComponent<PostProcessingBehaviour>();
            if (m_postProcessingBehaviour)
            {
                m_postProcessingBehaviour.enabled = false;
            }
        }
    }

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
        if (playerLogic)
        {
            playerLogic.Teleport();
            if (m_daySkybox)
            {
                RenderSettings.skybox = m_daySkybox;
            }

            if (m_postProcessingBehaviour)
            {
                m_postProcessingBehaviour.enabled = true;
            }
        }
    }
}
