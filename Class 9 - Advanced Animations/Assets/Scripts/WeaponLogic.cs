using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

    private bool m_isReloading = false;

    private MeshRenderer m_impactBulletMeshRenderer;
    private LineRenderer m_lineRenderer;
    private AudioSource m_audioSource;
    private Animator m_animator;

    // Start is called before the first frame update
    void Start()
    {
        m_lineRenderer = GetComponent<LineRenderer>();
        m_audioSource = GetComponent<AudioSource>();
        m_animator = GetComponentInParent<Animator>(); // from a player's animator
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

        if (Input.GetButtonDown("Fire1") && m_shotCooldown <= 0.0f && !m_isReloading)
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

        if (Input.GetKeyDown(KeyCode.R) && !m_isReloading)
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
        if (m_animator)
        {
            m_animator.SetTrigger("Shoot");
        }
        --m_bulletCount;
        PlaySound(m_shotSound);
    }

    void Reload()
    {
        if (m_animator)
        {
            m_animator.SetTrigger("Reload");
        }

        m_isReloading = true;
        PlaySound(m_reloadingSound);
        m_bulletCount = K_MAX_BULLETS;
    }

    // Added so we can change this state externally.
    // e.g. From behaviour script where we will handle OnStateExit case and will set m_isReloading to false after that animation is done.
    // That ReloadBehaviour script is attached to reload animation.
    public void SetReloadingState(bool state)
    {
        m_isReloading = state;
    }
}
