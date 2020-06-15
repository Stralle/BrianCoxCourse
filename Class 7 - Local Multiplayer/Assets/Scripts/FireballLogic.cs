using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballLogic : MonoBehaviour
{
    private Rigidbody m_rigidBody;
    [SerializeField] private float m_fireballSpeed = 8.0f;

    // Start is called before the first frame update
    void Start()
    {
        m_rigidBody = GetComponent<Rigidbody>();
        if (m_rigidBody)
        {
            m_rigidBody.velocity = transform.forward * m_fireballSpeed;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
