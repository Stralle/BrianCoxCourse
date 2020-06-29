using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class CustomMenuItems : Editor
{
    [MenuItem("Tools/PlayerPrefs/Delete All")]
    private static void DeleteAllPlayerPrefs()
    {
        PlayerPrefs.DeleteAll(); 
    }

    [MenuItem("Tools/Spawn/Cube")]
    private static void SpawnCube()
    {
        GameObject.CreatePrimitive(PrimitiveType.Cube);
    }

    [MenuItem("Tools/Spawn/Ground Platform")]
    private static void SpawnGroundPlatform()
    {
        GameObject basePlatform = GameObject.CreatePrimitive(PrimitiveType.Cube);
        basePlatform.transform.localScale = new Vector3(30, 1, 30);
    }
}
