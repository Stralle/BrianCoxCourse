using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RenameObjectsWindow : EditorWindow
{
    [MenuItem("Tools/Rename Objects/Rename Object From Selection")]
    static void RenameObject()
    {
        EditorWindow window = GetWindow(typeof(RenameObjectsWindow));
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("Select an object in the scene hierarchy that you want to rename.");
        if (Selection.activeGameObject)
        {
            Selection.activeGameObject.name =
                EditorGUILayout.TextField("Object name: ", Selection.activeGameObject.name);
        }

        Repaint(); // Refresh the window GUI. 
    }
}

public class RenameMultipleObjectsWindow : EditorWindow
{
    private string m_inputName = "";

    [MenuItem("Tools/Rename Objects/Rename Multiple Objects From Selection")]
    static void RenameMultipleObjects()
    {
        EditorWindow window = GetWindow(typeof(RenameMultipleObjectsWindow));
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("Select all objects in the scene hierarchy that you want to rename.");

        m_inputName = EditorGUILayout.TextField("Object name: ", Selection.activeGameObject.name);

        int numObjectsSelected = Selection.gameObjects.Length;

        if (numObjectsSelected > 0)
        {
            for (int index = 0; index < numObjectsSelected; index++)
            {
                Selection.gameObjects[index].name = m_inputName;
            }
        }

        Repaint(); // Refresh the window GUI. 
    }
}
