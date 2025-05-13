using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* NOMBRE CLASE: RiverFallEvent
 * AUTOR: Jone Sainz Egea
 * FECHA: 27/03/2025
 * DESCRIPCI�N: Script base que se encarga de que si el jugador cae al r�o reaparezca en la playa del principio
 * VERSI�N: 1.0 funcionamiento del fade in/fade out b�sico
 */
public class RiverFallEvent : MonoBehaviour
{
    [SerializeField] Transform beginningPoint;
    private Transform player;

    [SerializeField] GameObject camGO;
    private CameraFade cam;

    private void Start()
    {
        cam = camGO.GetComponent<CameraFade>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // TODO: don't allow Brisa to move!!!!
            player = other.transform;
            StartCoroutine(SendPlayerToBeginning(player));
        }
    }

    IEnumerator SendPlayerToBeginning(Transform _playerTransform)
    {
        // TODO: some animation
        cam.DoFadeInOut();
        yield return new WaitForSeconds(1f);
        _playerTransform.position = beginningPoint.position;
        yield return new WaitForSeconds(0.5f);
        cam.DoFadeInOut();
    }
}
