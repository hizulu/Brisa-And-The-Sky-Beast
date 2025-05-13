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
            Debug.Log("Detecta objeto hitteable");
            hittable.OnHit();
        }
    }
}
