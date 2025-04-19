using UnityEngine;
using UnityEngine.EventSystems;

public class MapNavigation : MonoBehaviour, IDragHandler, IScrollHandler
{
    [SerializeField] private Camera mapCamera;
    [SerializeField] private float dragSpeed = 1f;
    [SerializeField] private float zoomSpeed = 5f;
    [SerializeField] private float minZoom = 20f;
    [SerializeField] private float maxZoom = 100f;

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 drag = new Vector3(-eventData.delta.x, 0, -eventData.delta.y) * dragSpeed * Time.unscaledDeltaTime;
        mapCamera.transform.Translate(drag, Space.World);
    }

    public void OnScroll(PointerEventData eventData)
    {
        float zoomDelta = eventData.scrollDelta.y * zoomSpeed;
        float newSize = Mathf.Clamp(mapCamera.orthographicSize - zoomDelta, minZoom, maxZoom);
        mapCamera.orthographicSize = newSize;
    }
}
