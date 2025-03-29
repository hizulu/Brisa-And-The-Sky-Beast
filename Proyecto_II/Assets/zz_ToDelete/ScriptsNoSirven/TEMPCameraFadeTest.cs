using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEMPCameraFadeTest : MonoBehaviour
{
    public GameObject camGO;
    private CameraFade cam;
    private void Start()
    {
        cam = camGO.GetComponent<CameraFade>();
    }
    private void OnTriggerEnter(Collider other)
    {
        cam.DoFadeInOut();
    }
}
