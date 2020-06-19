using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScript : MonoBehaviour
{
    public void OnStartClicked()
    {
        Debug.Log("Start clicked");
    }

    public void OnOptionsClicked()
    {
        Debug.Log("Options clicked");
    }

    public void OnQuitClicked()
    {
        Debug.Log("Quit clicked");
    }
}
