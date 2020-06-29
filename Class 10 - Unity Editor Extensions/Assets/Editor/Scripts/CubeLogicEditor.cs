using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CubeLogic))] // We're saying that this custom editor script is type of CubeLogic.
public class CubeLogicEditor : Editor
{
    private Color m_color;
    private float m_cubeSize = 1.0f;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        CubeLogic cubeLogic = (CubeLogic) target; // We're getting a CubeLogic from a target that's supposed to be typeof CubeLogic

        GUILayout.Label("Press the button below to generate a random color!");
        if (GUILayout.Button("Generate Color"))
        {
            if (cubeLogic)
            {
                cubeLogic.GenerateColor();
            }

        }

        m_color = EditorGUILayout.ColorField("Select Your Color", m_color);
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Set ColorField Color"))
        {
            if (cubeLogic)
            {
                cubeLogic.SetColor(m_color);
            }
        }

        if (GUILayout.Button("Reset ColorField Color"))
        {
            if (cubeLogic)
            {
                cubeLogic.SetColor(Color.white);
            }
        }

        GUILayout.EndHorizontal();

        m_cubeSize = EditorGUILayout.Slider(m_cubeSize, 1, 5);
        if (cubeLogic)
        {
            cubeLogic.SetScale(m_cubeSize);
        }
    }
}
