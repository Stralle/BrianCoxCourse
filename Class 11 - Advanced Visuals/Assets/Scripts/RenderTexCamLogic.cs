using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderTexCamLogic : MonoBehaviour
{
    private Transform m_portalTransform;
    private Transform m_playerTransform;

    private bool m_canMove = false;
    private float m_posY;

    private const float K_MIN_DISTANCE = 8.0f;
    void Start()
    {
        m_portalTransform = GameObject.FindGameObjectWithTag("Portal").transform;
        m_playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        m_posY = transform.position.y; // Base camera position
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(m_portalTransform.position, m_playerTransform.position) < K_MIN_DISTANCE)
        {
            m_canMove = true;
        }
        else
        {
            m_canMove = false;
        }
    }

    public void Move(Vector3 movement)
    {
        if (m_canMove)
        {
            transform.Translate(movement); // translate camera
            Vector3 tempPos = transform.position;
            tempPos.y = m_posY; // setting back the Y pos.
            transform.position = tempPos;
        }
    }
}
