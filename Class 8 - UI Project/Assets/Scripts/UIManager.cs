using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EUIState
{
    MainMenu,
    OptionsMenu,
    VideoOptions,
    AudioOptions
}

[System.Serializable]
public struct UIStateStruct
{
    public EUIState m_uiState;
    public GameObject m_uiObject;
}

public class UIManager : MonoBehaviour
{
    public static UIManager Instance = null;
    [SerializeField]
    List<UIStateStruct> m_uiStates = new List<UIStateStruct>();
    
    Dictionary<EUIState, GameObject> m_uiStateMap = new Dictionary<EUIState, GameObject>();

    private EUIState m_currentUiState = EUIState.MainMenu;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        foreach (UIStateStruct uiStateStruct in m_uiStates)
        {
            m_uiStateMap.Add(uiStateStruct.m_uiState, uiStateStruct.m_uiObject);
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetUIState(EUIState newState)
    {
        m_uiStateMap[m_currentUiState].SetActive(false);
        m_uiStateMap[newState].SetActive(true);
        m_currentUiState = newState;
    }
}
