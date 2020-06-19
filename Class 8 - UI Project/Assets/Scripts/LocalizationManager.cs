using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public enum ELanguage
{
    English,
    Serbian
}
public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager instance = null;

    [SerializeField]
    private ELanguage m_currentLanguage = ELanguage.English;

    private Dictionary<ELanguage, TextAsset> m_localizationFiles = new Dictionary<ELanguage, TextAsset>();
    private Dictionary<string, string> m_localizationData = new Dictionary<string, string>();

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

        // Localization files/data should be loaded in Awake, to make sure everything is loaded on time!
        DontDestroyOnLoad(gameObject);
        SetupLocalizationFiles();
        SetupLocalizationData();
    }

    void SetupLocalizationFiles()
    {
        foreach (ELanguage language in ELanguage.GetValues(typeof(ELanguage)))
        {
            string textAssetPath = "Localization/" + language; 
            TextAsset textAsset = (TextAsset) Resources.Load(textAssetPath); // It looks for the file in the folder Resources by default, so now the path is Resources/Localization
            if (textAsset)
            {
                m_localizationFiles[language] = textAsset;
                Debug.Log("Text Asset: " + textAsset.name);
            }
            else
            {
                Debug.LogError("Text Asset not found " + textAssetPath);
            }
        }
    }

    void SetupLocalizationData()
    {
        TextAsset textAsset;
        if (m_localizationFiles.ContainsKey(m_currentLanguage))
        {
            Debug.Log("Selected language found: " + m_currentLanguage);
            textAsset = m_localizationFiles[m_currentLanguage];
        }
        else
        {
            Debug.LogError("Couldn't find localization file for: " + m_currentLanguage);
            textAsset = m_localizationFiles[ELanguage.English];
        }

        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(textAsset.text);

        XmlNodeList entryList = xmlDocument.GetElementsByTagName("Entry");

        foreach (XmlNode entry in entryList)
        {
            if (!m_localizationData.ContainsKey(entry.FirstChild.InnerText))
            {
                Debug.Log("Added key " + entry.FirstChild.InnerText + " with value " + entry.LastChild.InnerText);
                m_localizationData.Add(entry.FirstChild.InnerText, entry.LastChild.InnerText);
            }
            else
            {
                Debug.LogError("Duplicate key found for: " + entry.FirstChild.InnerText);
            }
        }
    }

    public string GetLocalizedString(string key)
    {
        string localizedString = "";
        if (!m_localizationData.TryGetValue(key, out localizedString))
        {
            localizedString = "LOC KEY: " + key;
        }

        return localizedString;
    }
}
