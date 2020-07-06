using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonLogic : MonoBehaviour
{
    private float m_rotationX = 0.0f;
    private const float K_MIN_X = -50.0f;
    private const float K_MAX_X = 50.0f;
    private const float K_ROTATION_SPEED = 2.0f;

    private PlayerLogic m_playerLogic;
    void Start()
    {
        m_playerLogic = GetComponentInParent<PlayerLogic>();
    }

    // Update is called once per frame
    void Update()
    {
        m_rotationX -= Input.GetAxis("Mouse Y") * K_ROTATION_SPEED;
        m_rotationX = Mathf.Clamp(m_rotationX, K_MIN_X, K_MAX_X);
    }

    void LateUpdate()
    {
        if (m_playerLogic)
        {
            transform.rotation = Quaternion.Euler(m_rotationX, m_playerLogic.GetRotationY(), 0); // Have to add rotation Y for FPS container as well!
        }
    }
}
