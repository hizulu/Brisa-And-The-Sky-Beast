using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        HittableElement hittable = other.GetComponent<HittableElement>();
        if (hittable != null)
        {
            hittable.OnHit();
        }
        else
            Debug.Log("No se ha detectado un objeto con el que colisionar");
    }
}
