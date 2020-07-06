using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    private float m_rotationY;
    private const float K_ROTATION_SPEED = 2.0f;

    void Start()
    {
        
    }

    void Update()
    {
        m_rotationY += Input.GetAxis("Mouse X") * K_ROTATION_SPEED;

        transform.rotation = Quaternion.Euler(0, m_rotationY, 0);
    }

    public float GetRotationY()
    {
        return m_rotationY;
    }
}
