using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ProceduralGenerationWindow : EditorWindow
{
    private string m_minX = "0", m_maxX = "0";
    private string m_minY = "0", m_maxY = "0";
    private string m_minZ = "0", m_maxZ = "0";

    private string m_minRotX = "0";
    private string m_maxRotX = "0";

    private string m_minRotY = "0";
    private string m_maxRotY = "0";

    private string m_minRotZ = "0";
    private string m_maxRotZ = "0";

    [MenuItem("Tools/Spawn/Procedural Generation")]
    static void SpawnObject()
    {
        EditorWindow window = GetWindow(typeof(ProceduralGenerationWindow));
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("Select an object you would like to spawn.");

        if (!Selection.activeGameObject)
        {
            return;
        }

        GUILayout.Label("You have selected: " + Selection.activeGameObject.name); 

        SetupPosition();
        SetupRotation();

        if (GUILayout.Button("Spawn Object"))
        {
            SpawnObjects();
        }

        Repaint(); // Refresh the window GUI. 
    }

    void SetupPosition()
    {
        GUILayout.Label("Where do you want to spawn the objects?");

        m_minX = EditorGUILayout.TextField("Min X: ", m_minX);
        m_maxX = EditorGUILayout.TextField("Max X: ", m_maxX);

        m_minY = EditorGUILayout.TextField("Min Y: ", m_minY);
        m_maxY = EditorGUILayout.TextField("Max Y: ", m_maxY);

        m_minZ = EditorGUILayout.TextField("Min Z: ", m_minZ);
        m_maxZ = EditorGUILayout.TextField("Max Z: ", m_maxZ);
    }

    void SetupRotation()
    {
        GUILayout.Label("Determine the ROTATION of the spawned objects?");

        m_minRotX = EditorGUILayout.TextField("Min X: ", m_minRotX);
        m_maxRotX = EditorGUILayout.TextField("Max X: ", m_maxRotX);

        m_minRotY = EditorGUILayout.TextField("Min Y: ", m_minRotY);
        m_maxRotY = EditorGUILayout.TextField("Max Y: ", m_maxRotY);

        m_minRotZ = EditorGUILayout.TextField("Min Z: ", m_minRotZ);
        m_maxRotZ = EditorGUILayout.TextField("Max Z: ", m_maxRotZ);
    }

    void SpawnObjects()
    {
        float posX = Random.Range(float.Parse(m_minX), float.Parse(m_maxX));
        float posY = Random.Range(float.Parse(m_minY), float.Parse(m_maxY));
        float posZ = Random.Range(float.Parse(m_minZ), float.Parse(m_maxZ));

        float rotX = Random.Range(float.Parse(m_minRotX), float.Parse(m_maxRotX));
        float rotY = Random.Range(float.Parse(m_minRotY), float.Parse(m_maxRotY));
        float rotZ = Random.Range(float.Parse(m_minRotZ), float.Parse(m_maxRotZ));

        Quaternion rotation = Quaternion.Euler(new Vector3(rotX, rotY, rotZ));

        Instantiate(Selection.activeGameObject, new Vector3(posX, posY, posZ), rotation, null);
    }
}
