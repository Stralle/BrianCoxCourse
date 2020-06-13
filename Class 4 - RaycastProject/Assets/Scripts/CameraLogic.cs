using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLogic : MonoBehaviour
{
    // Camera offsetting values
    float m_cameraMovementOffset = 0.15f;
    float m_cameraCheckHeightOffset = 0.0f;
    float m_cameraCheckWidthOffset = 0.0f;
    float m_divideCameraCheckBy = 7.5f;
    float m_cameraCenteringZOffset = 7.0f;

    // Player holder
    GameObject m_playerObject;

    void Start()
    {
        m_playerObject = GameObject.FindGameObjectWithTag("Player");

        // Setting camera offset for smoother movement with better feeling.
        m_cameraCheckHeightOffset = Screen.width / m_divideCameraCheckBy;
        m_cameraCheckWidthOffset = Screen.height / m_divideCameraCheckBy;
    }

    void Update()
    {
        UpdateCameraPosition();

        if (Input.GetKey(KeyCode.Space)) // Not KeyDown because we want to be able to hold space and continuously follow a player
        {
            CenterCamera();
        }
    }

    void UpdateCameraPosition()
    {
        if (Input.mousePosition.x >= (Screen.width - m_cameraCheckWidthOffset)) // Move the camera to the RIGHT 
        {
            transform.position = new Vector3(transform.position.x + m_cameraMovementOffset, transform.position.y, transform.position.z);
        }
        else if (Input.mousePosition.x <= (0.0f + m_cameraCheckWidthOffset)) // Move the camera to the LEFT
        {
            transform.position = new Vector3(transform.position.x - m_cameraMovementOffset, transform.position.y, transform.position.z);
        }

        if (Input.mousePosition.y >= (Screen.height - m_cameraCheckHeightOffset)) // Move the camera UP
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + m_cameraMovementOffset);
        }
        else if (Input.mousePosition.y <= (0.0f + m_cameraCheckHeightOffset)) // Move the camera DOWN
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - m_cameraMovementOffset);
        }
    }

    void CenterCamera()
    {
        // Camera needs to be slightly lower than a player on a Z axis because of the camera view.
        transform.position = new Vector3(m_playerObject.transform.position.x, transform.position.y, m_playerObject.transform.position.z - m_cameraCenteringZOffset); 
    }
}
