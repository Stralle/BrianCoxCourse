using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponLogic : MonoBehaviour
{
    [SerializeField] private Transform m_bulletSpawnPosition;
    [SerializeField] private float m_lineRendererLength = 1.0f;

    private LineRenderer m_lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        m_lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLineRenderer();
    }

    void UpdateLineRenderer()
    {
        if (m_lineRenderer)
        {
            m_lineRenderer.SetPosition(0, m_bulletSpawnPosition.position);
            m_lineRenderer.SetPosition(1, m_bulletSpawnPosition.position + m_bulletSpawnPosition.transform.forward * m_lineRendererLength);
        }
    }
}
