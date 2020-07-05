using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLogic : MonoBehaviour
{
    [SerializeField]
    float m_cameraTargetYOffset = 2.0f;
    [SerializeField]
    float m_distanceZOffset = -6.0f;
    [SerializeField]
    float m_rotationX = 6.1f;
    [SerializeField]
    float m_rotationY = 0.0f;

    Vector3 m_cameraTarget;
    GameObject m_player;
    const float MIN_X = -25.0f;
    const float MAX_X = 25.0f;
    const float MIN_Z = -8f;
    const float MAX_Z = -3.2f;

    private bool m_isAiming = false;

    private float m_aimPosX = 0.65f;
    private float m_aimPosY = 1.55f;
    private float m_aimPosZ = -0.7f;

    private float m_aimRotationY;

    void Start()
    {
        m_player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        m_cameraTarget = m_player.transform.position;
        m_cameraTarget.y += m_cameraTargetYOffset;

        if (Input.GetButtonDown("Fire3"))
        {
            m_isAiming = !m_isAiming;

            if (m_isAiming)
            {
                m_aimRotationY = m_rotationY;
                m_player.transform.rotation = Quaternion.Euler(0, m_aimRotationY, 0);
            }
            else
            {
                m_rotationY = m_aimRotationY;
            }
            
        }

        if (Input.GetButton("Fire2"))
        {
            m_rotationY += Input.GetAxis("Mouse X"); // Rotating around the Y axis.
            m_rotationX -= Input.GetAxis("Mouse Y"); // Rotating around the X axis
        }

        m_distanceZOffset += Input.GetAxis("Mouse ScrollWheel");
        m_distanceZOffset = Mathf.Clamp(m_distanceZOffset, MIN_Z, MAX_Z);
        m_rotationX = Mathf.Clamp(m_rotationX, MIN_X, MAX_X);
    }

    private void LateUpdate()
    {
        if (!m_isAiming)
        {
            Quaternion cameraRotation = Quaternion.Euler(m_rotationX, m_rotationY, 0);
            Vector3 cameraOffset = new Vector3(0, 0, m_distanceZOffset);
            transform.position = m_cameraTarget + cameraRotation * cameraOffset; // Order is important. Multiply rotation * offset and not vice versa!
            transform.LookAt(m_cameraTarget);
        }
        else
        {
            m_cameraTarget = m_player.transform.position;
            Vector3 cameraOffset = new Vector3(m_aimPosX, m_aimPosY, m_aimPosZ);

            Quaternion cameraRotation = m_player.transform.rotation;

            transform.position = m_cameraTarget + cameraRotation * cameraOffset;
            transform.rotation = Quaternion.Euler(0, m_aimRotationY, 0);  
        }
    }

    public Vector3 GetForwardVector()
    {
        Quaternion rotation = Quaternion.Euler(0, m_rotationY, 0);
        return rotation * Vector3.forward;
    }

    public float GetRotationX()
    {
        return m_rotationX;
    }
}
