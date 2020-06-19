using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuScript : MonoBehaviour
{
    [SerializeField] private Button m_focusButton; // To choose from what button we want to start with.

    void Start()
    {
        if (m_focusButton)
        {
            m_focusButton.Select(); // This is used to support a keyboard/controllers in main menu without mouse.
        }
    }
    public void OnVideoOptionsClicked()
    {
        UIManager.Instance.SetUIState(EUIState.VideoOptions);
        Debug.Log("Video options clicked");
    }

    public void OnAudioOptionsClicked()
    {
        UIManager.Instance.SetUIState(EUIState.AudioOptions);
        Debug.Log("Audio options clicked");
    }

    public void OnBackClicked()
    {
        Debug.Log("Back clicked");
        UIManager.Instance.SetUIState(EUIState.MainMenu);
    }
}
