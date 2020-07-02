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

    private string m_dialogMessage;
    private int m_dialogIndex;
    private const float K_DIALOG_LETTER_COOLDOWN = 0.04f;
    private float m_dialogTimerLetter;

    private bool m_isMessageFullyShown = false;

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
            if (!m_isMessageFullyShown)
            {
                m_dialogTimerLetter -= Time.deltaTime;
                if (m_dialogTimerLetter <= 0.0f)
                {
                    m_dialogTimerLetter = K_DIALOG_LETTER_COOLDOWN;
                    ShowNextLetter();
                }
            }
            else
            {
                m_displayTime -= Time.deltaTime;
                if (m_displayTime <= 0.0f)
                {
                    m_dialogWindow.SetActive(false);
                    m_isDisplayed = false;
                }
            }
        }
    }

    private void ShowNextLetter()
    {
        ++m_dialogIndex;
        if (m_dialogText && m_dialogMessage != "" && m_dialogMessage.Length > m_dialogIndex)
        {
            if (m_dialogMessage[m_dialogIndex] == '<' && m_dialogMessage.Substring(m_dialogIndex, 4) == "<br>")
            {
                m_dialogText.text = m_dialogMessage.Substring(0, m_dialogIndex + 4);
                m_dialogIndex += 4;
            }
            else
            {
                m_dialogText.text = m_dialogMessage.Substring(0,m_dialogIndex);
            }
        }
        else
        {
            m_isMessageFullyShown = true;
        }
    }

    public void SetText(string characterName, string dialogText, float displayTime)
    {
        if (m_characterName)
        {
            m_characterName.text = characterName;
        }

        if (m_dialogText)
        {
            m_dialogText.text = "";
        }

        m_dialogWindow.SetActive(true);
        m_dialogMessage = dialogText;

        m_isDisplayed = true;
        m_displayTime = displayTime;

        m_isMessageFullyShown = false;
        m_dialogIndex = 0;
    }
}