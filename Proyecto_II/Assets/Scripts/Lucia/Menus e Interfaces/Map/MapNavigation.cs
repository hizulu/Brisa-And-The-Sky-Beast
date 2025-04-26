using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Camera))] // Asegura que haya una cámara
public class MapNavigation : MonoBehaviour, IDragHandler, IScrollHandler
{
    [SerializeField] private Camera mapCamera;
    [SerializeField] public float dragSpeed = 1f;
    [SerializeField] private float zoomSpeed = 5f;
    [SerializeField] private float minZoom = 20f;
    [SerializeField] private float maxZoom = 100f;

    private void Awake()
    {
        // Si no se asignó manualmente la cámara, usa la del mismo GameObject
        if (mapCamera == null)
        {
            mapCamera = GetComponent<Camera>();
            if (mapCamera == null)
            {
                Debug.LogError("No se encontró componente Camera en el GameObject", this);
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (mapCamera == null) return;

        Vector3 drag = new Vector3(-eventData.delta.x, 0, -eventData.delta.y) * dragSpeed * Time.unscaledDeltaTime;
        mapCamera.transform.Translate(drag, Space.World);
    }

    public void OnScroll(PointerEventData eventData)
    {
        if (mapCamera == null || !mapCamera.orthographic) return;

        float zoomDelta = eventData.scrollDelta.y * zoomSpeed;
        float newSize = Mathf.Clamp(mapCamera.orthographicSize - zoomDelta, minZoom, maxZoom);
        mapCamera.orthographicSize = newSize;
    }
}