using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private int m_p1Score = 0;
    private int m_p2Score = 0;

    [SerializeField] private Text m_p1ScoreText;
    [SerializeField] private Text m_p2ScoreText;

    void OnEnable()
    {
        PlayerLogic.OnPlayerDeath += OnUpdateScore;
    }

    void OnDisable()
    {
        PlayerLogic.OnPlayerDeath -= OnUpdateScore;
    }

    void OnUpdateScore(int playerNum)
    {
        if (playerNum == 2) // If player 2 gets hit, then we want to add a score to a player one.
        {
            ++m_p1Score;
            m_p1ScoreText.text = "" + m_p1Score;
        }
        else if (playerNum == 1)
        {
            ++m_p2Score;
            m_p2ScoreText.text = "" + m_p2Score;
        }
    }
}
