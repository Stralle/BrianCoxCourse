using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponLogic : MonoBehaviour
{
    [SerializeField] private Transform m_bulletSpawnPosition;
    [SerializeField] private float m_lineRendererLength = 1.0f;
    [SerializeField] private GameObject m_impactPos;
    [SerializeField] private AudioClip m_shotSound;
    [SerializeField] private AudioClip m_emptyGunSound;
    [SerializeField] private AudioClip m_reloadingSound;


    private const int K_MAX_BULLETS = 30;
    private int m_bulletCount = K_MAX_BULLETS;

    private const float K_MAX_COOLDOWN = 0.1f;
    private float m_shotCooldown = K_MAX_COOLDOWN;


    private MeshRenderer m_impactBulletMeshRenderer;
    private LineRenderer m_lineRenderer;
    private AudioSource m_audioSource;

    // Start is called before the first frame update
    void Start()
    {
        m_lineRenderer = GetComponent<LineRenderer>();
        m_audioSource = GetComponent<AudioSource>();
        if (m_impactPos)
        {
            m_impactBulletMeshRenderer = m_impactPos.GetComponent<MeshRenderer>();
            m_impactBulletMeshRenderer.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLineRenderer();

        if (m_shotCooldown > 0)
        {
            m_shotCooldown -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Fire1") && m_shotCooldown <= 0.0f)
        {
            if (m_bulletCount > 0)
            {
                Shoot();
            }

            else
            {
                // Empty gun
                PlaySound(m_emptyGunSound);
            }

            m_shotCooldown = K_MAX_COOLDOWN;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
    }

    void UpdateLineRenderer()
    {
        if (m_lineRenderer)
        {
            m_lineRenderer.SetPosition(0, m_bulletSpawnPosition.position);

            Ray ray = new Ray(m_bulletSpawnPosition.position, m_bulletSpawnPosition.transform.forward);
            RaycastHit rayHit;
            if (Physics.Raycast(ray, out rayHit, m_lineRendererLength))
            {
                m_lineRenderer.SetPosition(1, rayHit.point);
                m_impactPos.transform.position = rayHit.point;
                m_impactBulletMeshRenderer.enabled = true;
            }
            else
            {
                m_lineRenderer.SetPosition(1, m_bulletSpawnPosition.position + m_bulletSpawnPosition.transform.forward * m_lineRendererLength);
                m_impactBulletMeshRenderer.enabled = false;
            }

        }
    }

    void PlaySound(AudioClip audioClip)
    {
        if (m_audioSource && audioClip)
        {
            m_audioSource.PlayOneShot(audioClip);
        }
    }

    void Shoot()
    {
        --m_bulletCount;
        PlaySound(m_shotSound);
    }

    void Reload()
    {
        PlaySound(m_reloadingSound);
        m_bulletCount = K_MAX_BULLETS;
    }
}
