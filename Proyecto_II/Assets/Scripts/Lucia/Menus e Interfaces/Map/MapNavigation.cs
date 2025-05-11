using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Camera))] // Asegura que haya una cámara

/* 
 * NOMBRE CLASE: MapNavigation
 * AUTOR: Lucía García López
 * FECHA: 19/04/2025
 * DESCRIPCIÓN: Script que gestiona la navegación del mapa. Permite arrastrar y hacer zoom en el mapa.
 * VERSIÓN: 1.0 Sistema de navegación del mapa inicial.
 * 1.1 La sensibilidad ajusta el valor de dragSpeed.
 */

public class MapNavigation : MonoBehaviour, IDragHandler, IScrollHandler
{
    #region Variables
    [SerializeField] private Camera mapCamera;
    [SerializeField] public float dragSpeed = 1f;
    [SerializeField] private float zoomSpeed = 5f;
    [SerializeField] private float minZoom = 20f;
    [SerializeField] private float maxZoom = 100f;
    #endregion

    private void Awake()
    {
        if (mapCamera == null)
        {
            mapCamera = GetComponent<Camera>();
            if (mapCamera == null)
            {
                Debug.LogError("No se encontró componente Camera en el GameObject", this);
            }
        }
    }

    // Método para arrastrar el mapa
    public void OnDrag(PointerEventData eventData)
    {
        if (mapCamera == null) return;

        Vector3 drag = new Vector3(-eventData.delta.x, 0, -eventData.delta.y) * dragSpeed * Time.unscaledDeltaTime;
        mapCamera.transform.Translate(drag, Space.World);
    }

    // Método para hacer zoom en el mapa
    public void OnScroll(PointerEventData eventData)
    {
        if (mapCamera == null || !mapCamera.orthographic) return;

        float zoomDelta = eventData.scrollDelta.y * zoomSpeed;
        float newSize = Mathf.Clamp(mapCamera.orthographicSize - zoomDelta, minZoom, maxZoom);
        mapCamera.orthographicSize = newSize;
    }
}