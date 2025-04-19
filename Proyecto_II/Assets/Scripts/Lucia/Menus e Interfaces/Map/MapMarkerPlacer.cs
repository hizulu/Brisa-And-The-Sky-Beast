using UnityEngine;
using UnityEngine.EventSystems;

public class MapMarkerPlacer : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Camera mapCamera;
    [SerializeField] private GameObject markerPrefab;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!eventData.button.Equals(PointerEventData.InputButton.Left)) return;

        Vector2 localCursor;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform as RectTransform, eventData.position, eventData.pressEventCamera, out localCursor
        );

        Vector3 worldPos = mapCamera.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, mapCamera.transform.position.y));
        worldPos.y = 0f;

        Instantiate(markerPrefab, worldPos, Quaternion.identity);
    }
}
