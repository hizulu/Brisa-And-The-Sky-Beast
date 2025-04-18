using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEMPTeleportToPosition : MonoBehaviour
{
    [SerializeField] Transform teleportHere;
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
            StartCoroutine(SendPlayerToPosition(player));
        }
    }

    IEnumerator SendPlayerToPosition(Transform _playerTransform)
    {
        // TODO: some animation
        cam.DoFadeInOut();
        yield return new WaitForSeconds(1f);
        _playerTransform.position = teleportHere.position;
        yield return new WaitForSeconds(0.5f);
        cam.DoFadeInOut();
    }
}
