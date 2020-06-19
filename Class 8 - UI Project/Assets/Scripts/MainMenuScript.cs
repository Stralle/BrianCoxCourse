using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] private Button m_startButton; // To choose from what button we want to start with.

    void Start()
    {
        if (m_startButton)
        {
            m_startButton.Select(); // This is used to support a keyboard/controllers in main menu without mouse.
        }
    }
    public void OnStartClicked()
    {
        Debug.Log("Start clicked");
        SceneManager.LoadScene("GameScene");
    }

    public void OnOptionsClicked()
    {
        UIManager.Instance.SetUIState(EUIState.OptionsMenu);
        Debug.Log("Options clicked");
    }

    public void OnQuitClicked()
    {
        Debug.Log("Quit clicked");
        Application.Quit();
    }
}
