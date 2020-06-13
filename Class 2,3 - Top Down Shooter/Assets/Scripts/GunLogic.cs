using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunLogic : MonoBehaviour
{
    // Serialized fields
    [SerializeField]
    GameObject m_bulletPrefab;
    [SerializeField]
    Transform m_bulletSpawnPoint;
    [SerializeField]
    Text m_ammoText;
    [SerializeField]
    AudioClip m_pistolShot;
    [SerializeField]
    AudioClip m_pistolEmpty;
    [SerializeField]
    AudioClip m_pistolReload;
    [SerializeField]
    int m_ammoCount = MAX_AMMO;

    // Constants
    const int MAX_AMMO = 10;
    const float MAX_FIRE_COOLDOWN = 0.25f;
    
    // States
    float m_currentFireCooldown = 0.0f;
    bool m_isEquipped = false;

    // Components
    Rigidbody m_rigidBody;
    AudioSource m_audioSource;
    Collider m_collider;

    void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
        m_rigidBody = GetComponent<Rigidbody>();
        m_collider = GetComponent<Collider>();
        DefaultAmmoText();
    }

    void SetAmmoText()
    {
        if (m_ammoText)
        {
            m_ammoText.text = "Ammo: " + m_ammoCount;
        }
    }
    void DefaultAmmoText()
    {
        if (m_ammoText)
        {
            m_ammoText.text = "No weapon!";
        }
    }


    void Update()
    {
        if (!m_isEquipped)
        {
            return;
        }

        if (m_currentFireCooldown > 0.0f)
        {
            m_currentFireCooldown -= Time.deltaTime; // reducing cooldown
        }

        if (Input.GetButtonDown("Fire1") && m_bulletPrefab != null && m_currentFireCooldown <= 0.0f)
        {
            if (m_ammoCount > 0)
            {
                Instantiate(m_bulletPrefab, m_bulletSpawnPoint.position, m_bulletSpawnPoint.rotation * m_bulletPrefab.transform.rotation);

                m_currentFireCooldown = MAX_FIRE_COOLDOWN;
                m_ammoCount--;

                SetAmmoText();
                PlaySound(m_pistolShot);
            }
            else
            {
                PlaySound(m_pistolEmpty);
            }
        }
    }

    void PlaySound(AudioClip sound)
    {
        if (m_audioSource && sound)
        {
            m_audioSource.PlayOneShot(sound);
        }
    }

    // Refill ammo to max
    public void RefillAmmo()
    {
        m_ammoCount = MAX_AMMO;
        SetAmmoText();
        PlaySound(m_pistolReload);
    }

    public void EquipGun()
    {
        if (m_rigidBody)
        {
            m_rigidBody.useGravity = false;
        }
        if (m_collider)
        {
            m_collider.enabled = false;
        }
        m_isEquipped = true;
        SetAmmoText();
    }

    public void UnequipGun()
    {
        if (m_rigidBody)
        {
            m_rigidBody.useGravity = true;
        }
        if (m_collider)
        {
            m_collider.enabled = true;
        }
        m_isEquipped = false;
        DefaultAmmoText();
    }
}
