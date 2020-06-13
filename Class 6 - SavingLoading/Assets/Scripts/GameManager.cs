using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    private GameObject m_player;
    private PlayerLogic m_playerLogic;

    private GameObject[] m_coins;
    List<CoinLogic> m_coinLogics = new List<CoinLogic>();

    private GameObject[] m_enemies;
    private List<EnemyLogic> m_enemyLogics = new List<EnemyLogic>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        m_player = GameObject.FindGameObjectWithTag("Player");
        if (m_player)
        {
            m_playerLogic = m_player.GetComponent<PlayerLogic>();
        }

        m_coins = GameObject.FindGameObjectsWithTag("Coin");
        for (int index = 0; index < m_coins.Length; ++index)
        {
            m_coinLogics.Add(m_coins[index].GetComponent<CoinLogic>());
        }

        m_enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int index = 0; index < m_enemies.Length; ++index)
        {
            m_enemyLogics.Add(m_enemies[index].GetComponent<EnemyLogic>());
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Save();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            Load();
        }
    }

    public void Save()
    {
        m_playerLogic.Save();

        for (int index = 0; index < m_coinLogics.Count; ++index)
        {
            m_coinLogics[index].Save(index);
        }

        for (int index = 0; index < m_enemyLogics.Count; ++index)
        {
            m_enemyLogics[index].Save(index);
        }

        PlayerPrefs.Save();
    }

    public void Load()
    {
        m_playerLogic.Load();

        for (int index = 0; index < m_coinLogics.Count; ++index)
        {
            m_coinLogics[index].Load(index);
        }

        for (int index = 0; index < m_enemyLogics.Count; ++index)
        {
            m_enemyLogics[index].Load(index);
        }

    }
}
