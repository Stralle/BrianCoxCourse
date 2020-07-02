using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    public static DialogManager instance = null;

    [SerializeField] private TextMeshProUGUI m_characterName;
    [SerializeField] private TextMeshProUGUI m_dialogText;
    [SerializeField] private GameObject m_dialogWindow;

    private float m_displayTime;
    private bool m_isDisplayed = false;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (m_isDisplayed)
        {
            m_displayTime -= Time.deltaTime;

            if (m_displayTime <= 0.0f)
            {
                m_dialogWindow.SetActive(false);
                m_isDisplayed = false;
            }
        }
    }

    public void SetText(string characterName, string dialogText, float displayTime)
    {
        m_dialogWindow.SetActive(true);
        if (m_characterName)
        {
            m_characterName.text = characterName;
        }

        if (m_dialogText)
        {
            m_dialogText.text = dialogText;
        }

        m_isDisplayed = true;
        m_displayTime = displayTime;
    }

    //public void DisableDialogWindow()
    //{
    //    m_dialogWindow.SetActive(false);
    //}
}
