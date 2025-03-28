using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBox : HittableElement
{
    [SerializeField] private GameObject boxModel; // Referencia al objeto visual de la caja

    public override void OnHit()
    {
        Debug.Log("La caja se ha roto.");
        if (boxModel != null)
        {
            Destroy(boxModel); // Destruye la caja al ser golpeada
        }
    }
}
