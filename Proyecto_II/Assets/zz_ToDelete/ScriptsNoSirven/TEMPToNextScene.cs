using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Jone Sainz Egea
// 05/05/2025
public class TEMPToNextScene : MonoBehaviour
{
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
            StartCoroutine(SendPlayerToNextScene());
        }
    }

    IEnumerator SendPlayerToNextScene()
    {
        cam.DoFadeInOut();
        yield return new WaitForSeconds(1f);
        GameManager.Instance.LoadNextScene();
    }

}
