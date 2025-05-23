using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Jone Sainz Egea
// Script que detecta si se golpea algún elemento de tipo hittable
public class HitBox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        HittableElement hittable = other.GetComponent<HittableElement>();
        if (hittable != null)
        {
            Debug.Log("Detecta objeto hitteable");
            hittable.OnHit();
        }
    }
}
