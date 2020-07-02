using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTriggerBox : MonoBehaviour
{
    [SerializeField] private string m_characterName;
    [SerializeField] private string m_dialogText;
    [SerializeField] private float m_displayTime = 3.0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && DialogManager.instance)
        {
            DialogManager.instance.SetText(m_characterName, m_dialogText, m_displayTime);
        }
    }

}
