using UnityEngine;

public class AutoScrollBackground : MonoBehaviour
{
    private Camera mainCamera;
    private float cameraHalfWidth;

    [SerializeField] private float scrollSpeed = 5f;
    [SerializeField] private ParallaxLayer[] backgroundLayers;

    private void Awake()
    {
        mainCamera = Camera.main;
        cameraHalfWidth = mainCamera.orthographicSize * mainCamera.aspect;
        InitializeLayers();
    }

    private void LateUpdate()
    {
        float distanceToMove = scrollSpeed * Time.deltaTime;
        float currentCameraPositionX = mainCamera.transform.position.x;

        float cameraLeftEdge = currentCameraPositionX - cameraHalfWidth;
        float cameraRightEdge = currentCameraPositionX + cameraHalfWidth;

        foreach (ParallaxLayer layer in backgroundLayers)
        {
            layer.Move(distanceToMove);
            layer.LoopBackground(cameraLeftEdge, cameraRightEdge);
        }
    }

    private void InitializeLayers()
    {
        foreach (ParallaxLayer layer in backgroundLayers)
            layer.CalculateImageWidth();
    }
}