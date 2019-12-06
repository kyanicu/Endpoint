using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapOverlayManager : MonoBehaviour
{
    [Tooltip("Drag in the main camera prefab from the scene")]
    public GameObject MainCameraPrefab;
    private Camera mapCamera;
    private float cameraSpeed = 50f;
    private  Vector3 startPos; 

    #region map camera constants
    private const float MAX_ZOOM_OUT = 75;
    private const float MAX_ZOOM_IN = 15;
    private const float DEFAULT_ZOOM = 35;
    private const float ZOOM_INCREMENT = .5f;
    private const float startingZ = -13;
    #endregion

    /// <summary>
    /// Retrieve the map overlay camera on component awake
    /// </summary>
    private void Awake()
    {
        mapCamera = MainCameraPrefab.transform.Find("MapOverlayCamera").GetComponent<Camera>();
    }

    private void OnEnable()
    {
        if (Player.instance != null)
        {
            startPos = Player.instance.transform.position;
            startPos = new Vector3(startPos.x, startPos.y, startingZ);
            ResetCamera();
        }
    }

    /// <summary>
    /// Moves the player controlled camera on the map overlay panel
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void MoveCamera(float x, float y)
    {
        float newX = x * cameraSpeed * Time.deltaTime;
        float newY = y * cameraSpeed * Time.deltaTime;
        mapCamera.transform.position += new Vector3(newX, newY, 0);
    }

    /// <summary>
    /// Zooms the camera in or out
    /// </summary>
    /// <param name="zoomIn"></param>
    public void ZoomCamera(bool zoomIn)
    {
        if (zoomIn && mapCamera.orthographicSize > MAX_ZOOM_IN)
        {
            mapCamera.orthographicSize -= ZOOM_INCREMENT;
        }
        else if (!zoomIn && mapCamera.orthographicSize < MAX_ZOOM_OUT)
        {
            mapCamera.orthographicSize += ZOOM_INCREMENT;
        }
    }

    /// <summary>
    /// Resets zoom and position of camera
    /// </summary>
    public void ResetCamera()
    {
        mapCamera.transform.position = startPos;
        mapCamera.orthographicSize = DEFAULT_ZOOM;
    }
}
