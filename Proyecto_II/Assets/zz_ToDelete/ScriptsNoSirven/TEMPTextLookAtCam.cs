using UnityEngine;

public class TEMPTextLookAtCamera : MonoBehaviour
{
    private Camera mainCamera;
    private bool maintainVerticalOrientation = true;

    private void Start()
    {
        mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError("No se encontró la cámara principal!");
            enabled = false;
        }
    }

    private void LateUpdate()
    {
        if (mainCamera == null) return;
        Vector3 directionToCamera = mainCamera.transform.position - transform.position;

        if (maintainVerticalOrientation)
        {
            directionToCamera.y = 0;
        }

        if (directionToCamera != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(-directionToCamera);
        }
    }
}