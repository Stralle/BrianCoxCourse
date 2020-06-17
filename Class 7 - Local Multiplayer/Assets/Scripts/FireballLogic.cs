using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballLogic : MonoBehaviour
{
    private Rigidbody m_rigidBody;
    [SerializeField] private float m_fireballSpeed = 8.0f;
    [SerializeField] private ParticleSystem m_explosion;
    [SerializeField] private ParticleSystem m_fireball;
    private Collider m_collider;

    // Start is called before the first frame update
    void Start()
    {
        m_rigidBody = GetComponent<Rigidbody>();
        if (m_rigidBody)
        {
            m_rigidBody.velocity = transform.forward * m_fireballSpeed;
        }

        m_collider = GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerLogic playerLogic = other.GetComponent<PlayerLogic>();
            if (playerLogic)
            {
                playerLogic.Die();
            }
            m_fireball.Stop(true);
            m_explosion.Play(true);
            m_rigidBody.velocity = Vector3.zero;
            m_collider.enabled = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        m_collider.enabled = true;
    }
}
