using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Camera))] // Asegura que haya una c�mara

/* 
 * NOMBRE CLASE: MapNavigation
 * AUTOR: Luc�a Garc�a L�pez
 * FECHA: 19/04/2025
 * DESCRIPCI�N: Script que gestiona la navegaci�n del mapa. Permite arrastrar y hacer zoom en el mapa.
 * VERSI�N: 1.0 Sistema de navegaci�n del mapa inicial.
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
                Debug.LogError("No se encontr� componente Camera en el GameObject", this);
            }
        }
    }

    // M�todo para arrastrar el mapa
    public void OnDrag(PointerEventData eventData)
    {
        if (mapCamera == null) return;

        Vector3 drag = new Vector3(-eventData.delta.x, 0, -eventData.delta.y) * dragSpeed * Time.unscaledDeltaTime;
        mapCamera.transform.Translate(drag, Space.World);
    }

    // M�todo para hacer zoom en el mapa
    public void OnScroll(PointerEventData eventData)
    {
        if (mapCamera == null || !mapCamera.orthographic) return;

        float zoomDelta = eventData.scrollDelta.y * zoomSpeed;
        float newSize = Mathf.Clamp(mapCamera.orthographicSize - zoomDelta, minZoom, maxZoom);
        mapCamera.orthographicSize = newSize;
    }
}