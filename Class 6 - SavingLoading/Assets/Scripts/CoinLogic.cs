using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ECoinState
{
    Active,
    Inactive
}

public class CoinLogic : MonoBehaviour
{
    private Collider m_collider;
    private MeshRenderer m_meshRenderer;
    private AudioSource m_audioSource;
    [SerializeField]
    private AudioClip m_coinSound;

    private ECoinState m_coinState = ECoinState.Active;

    void Start()
    {
        m_collider = GetComponent<Collider>();
        m_meshRenderer = GetComponent<MeshRenderer>();
        m_audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        transform.Rotate(0,0,2);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            m_collider.enabled = false;
            m_meshRenderer.enabled = false;

            m_coinState = ECoinState.Inactive;

            PlaySound(m_coinSound);
        }
    }

    private void PlaySound(AudioClip sound)
    {
        if (m_audioSource && sound)
        {
            m_audioSource.PlayOneShot(sound);
        }
    }

    public void Save(int index)
    {
        PlayerPrefs.SetInt("CoinState" + index, (int) m_coinState); // We have to have unique identifier for each coin.
    }

    public void Load(int index)
    {
        m_coinState = (ECoinState) PlayerPrefs.GetInt("CoinState" + index);
        if (m_coinState == ECoinState.Active)
        {
            m_collider.enabled = true;
            m_meshRenderer.enabled = true;
        }
        else if (m_coinState == ECoinState.Inactive)
        {
            m_collider.enabled = false;
            m_meshRenderer.enabled = false;
        }
    }
}
