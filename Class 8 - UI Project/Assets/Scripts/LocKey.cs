using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This script should be attached to each gameobject we want to be localized!
public class LocKey : MonoBehaviour
{
    [SerializeField] private string m_locKey;

    private Text m_UIText;

    void Start()
    {
        m_UIText = GetComponent<Text>();

        if (m_UIText && m_locKey != "")
        {
            m_UIText.text = LocalizationManager.instance.GetLocalizedString(m_locKey);
        }
    }
}
