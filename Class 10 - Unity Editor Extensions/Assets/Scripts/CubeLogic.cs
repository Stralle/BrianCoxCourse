using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeLogic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GenerateColor();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void GenerateColor()
    {
        MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();

        if (meshRenderer)
        {
            meshRenderer.sharedMaterial.color = Random.ColorHSV();
        }
    }

    public void SetColor(Color color)
    {
        MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();

        if (meshRenderer)
        {
            meshRenderer.sharedMaterial.color = color;
        }
    }

    public void SetScale(float scale)
    {
        transform.localScale = Vector3.one * scale;
    }
}
