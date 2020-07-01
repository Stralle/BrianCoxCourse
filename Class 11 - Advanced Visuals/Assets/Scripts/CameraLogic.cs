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

    [SerializeField] private GameObject m_renderTextureCameraObject;

    Vector3 m_cameraTarget;
    GameObject m_player;
    const float MIN_X = -25.0f;
    const float MAX_X = 25.0f;
    const float MIN_Z = -8f;
    const float MAX_Z = -3.2f;

    void Start()
    {
        m_player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        m_cameraTarget = m_player.transform.position;
        m_cameraTarget.y += m_cameraTargetYOffset;

        if (Input.GetButton("Fire2"))
        {
            m_rotationY += Input.GetAxis("Mouse X"); // Rotating around the Y axis.
            m_rotationX -= Input.GetAxis("Mouse Y"); // Rotating around the X axis
        }

        m_distanceZOffset += Input.GetAxis("Mouse ScrollWheel");
        m_distanceZOffset = Mathf.Clamp(m_distanceZOffset, MIN_Z, MAX_Z);
        m_rotationX = Mathf.Clamp(m_rotationX, MIN_X, MAX_X);

        if (m_renderTextureCameraObject)
        {
            m_renderTextureCameraObject.transform.rotation = Quaternion.Euler(m_rotationX, m_rotationY,
                m_renderTextureCameraObject.transform.rotation.z);
        }
    }

    private void LateUpdate()
    {
        Quaternion cameraRotation = Quaternion.Euler(m_rotationX, m_rotationY, 0);
        Vector3 cameraOffset = new Vector3(0, 0, m_distanceZOffset);
        transform.position = m_cameraTarget + cameraRotation * cameraOffset; // Order is important. Multiply rotation * offset and not whice versa!
        transform.LookAt(m_cameraTarget);
    }

    public Vector3 GetForwardVector()
    {
        Quaternion rotation = Quaternion.Euler(0, m_rotationY, 0);
        return rotation * Vector3.forward;
    }
}
