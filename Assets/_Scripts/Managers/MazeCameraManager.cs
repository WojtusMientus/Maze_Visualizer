using UnityEngine;

public class MazeCameraManager : SingletonMonoBehaviour<MazeCameraManager>
{
    
    [SerializeField] private Camera mazeCamera;
    
    
    private void OnEnable()
    {
        EventManager.OnSizeSliderValueChanged += UpdateCameraPositionAndSize;
    }

    private void OnDisable()
    {
        EventManager.OnSizeSliderValueChanged -= UpdateCameraPositionAndSize;
    }
    
    private void UpdateCameraPositionAndSize(int newSize)
    {
        float newOrthoSize = newSize / 2f;
        
        mazeCamera.orthographicSize = newOrthoSize;
        
        Vector3 newPosition = new Vector3(newOrthoSize, newOrthoSize, 0f);
        mazeCamera.gameObject.transform.localPosition = newPosition; 
    }
}
