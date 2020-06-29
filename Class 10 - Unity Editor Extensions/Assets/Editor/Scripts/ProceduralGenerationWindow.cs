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

    private string m_amountOfObjects = "1";

    private GameObject m_spawnBox;

    private int m_selectionIndex;

    [MenuItem("Tools/Spawn/Procedural Generation")]
    static void SpawnObject()
    {
        EditorWindow window = GetWindow(typeof(ProceduralGenerationWindow));
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("How do you want to spawn the object(s)?");

        string[] selectionOptions = { "Enter Position Coordinates", "Select Spawn Box" };

        m_selectionIndex = GUILayout.SelectionGrid(m_selectionIndex, selectionOptions, selectionOptions.Length - 1);

        if (m_selectionIndex == 1)
        {
            SetupPositionFromBox();
            if (!m_spawnBox)
            {
                return;
            }
        }


        GUILayout.Label("Select the objects you would like to spawn.");
        if (Selection.gameObjects.Length <= 0)
        {
            return;
        }

        string selectedObjectNames = GetSelectedObjectNames(Selection.gameObjects);
        GUILayout.Label("You have selected: " + selectedObjectNames);

        if (m_selectionIndex == 0)
        {
            SetupPosition(); // We don't need this if we have a spawn box setup!
        }
        SetupRotation();

        SetupAmount();

        if (GUILayout.Button("Spawn Object"))
        {
            SpawnObjects(Selection.gameObjects, int.Parse(m_amountOfObjects));
        }

        Repaint(); // Refresh the window GUI. 
    }

    string GetSelectedObjectNames(GameObject[] selectedObjects)
    {
        string result = "";
        for (int i = 0; i < selectedObjects.Length; ++i)
        {
            result += selectedObjects[i].name;

            if (i < selectedObjects.Length - 1)
            {
                result += ",";
            }
            else
            {
                result += ".";
            }
        }

        return result;
    }

    void SetupPositionFromBox()
    {
        GUILayout.Label("Select spawning box.");

        if (!Selection.activeGameObject)
        {
            return;
        }

        if (GUILayout.Button("Set spawn box"))
        {
            m_spawnBox = Selection.activeGameObject;
        }

        if (!m_spawnBox)
        {
            return;
        }

        BoxCollider box = m_spawnBox.GetComponent<BoxCollider>();

        if (!box)
        {
            m_spawnBox = null;
            return;
        }

        m_minX = box.bounds.min.x.ToString();
        m_maxX = box.bounds.max.x.ToString();

        m_minY = box.bounds.min.y.ToString();
        m_maxY = box.bounds.max.y.ToString();

        m_minZ = box.bounds.min.z.ToString();
        m_maxZ = box.bounds.max.z.ToString();

        GUILayout.Label("Min X: " + m_minX);
        GUILayout.Label("Max X: " + m_maxX);

        GUILayout.Label("Min Y: " + m_minY);
        GUILayout.Label("Max Y: " + m_maxY);

        GUILayout.Label("Min Z: " + m_minZ);
        GUILayout.Label("Max Z: " + m_maxZ);
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

    void SetupAmount()
    {

        GUILayout.Label("How many objects do you want to spawn?");

        m_amountOfObjects = EditorGUILayout.TextField("Number of objects: ", m_amountOfObjects);
    }

    void SpawnObjects(GameObject[] gameObjects, int amount)
    {
        GameObject parentObj = new GameObject("SpawnParentObject");
        for (int index = 0; index < amount; ++index)
        {
            float posX = Random.Range(float.Parse(m_minX), float.Parse(m_maxX));
            float posY = Random.Range(float.Parse(m_minY), float.Parse(m_maxY));
            float posZ = Random.Range(float.Parse(m_minZ), float.Parse(m_maxZ));

            float rotX = Random.Range(float.Parse(m_minRotX), float.Parse(m_maxRotX));
            float rotY = Random.Range(float.Parse(m_minRotY), float.Parse(m_maxRotY));
            float rotZ = Random.Range(float.Parse(m_minRotZ), float.Parse(m_maxRotZ));

            Quaternion rotation = Quaternion.Euler(new Vector3(rotX, rotY, rotZ));

            int randomObjectIndex = Random.Range(0, gameObjects.Length);

            Ray ray = new Ray(new Vector3(posX, posY, posZ), Vector3.down);
            RaycastHit rayHit;

            if (Physics.Raycast(ray, out rayHit, 100.0f))
            {
                Instantiate(gameObjects[randomObjectIndex], rayHit.point, rotation, parentObj.transform);
            }
        }
    }
}
