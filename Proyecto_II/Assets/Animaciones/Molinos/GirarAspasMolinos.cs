using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirarAspasMolinos : MonoBehaviour
{
    public GameObject[] objetosARotar;
    public float velocidadRotacionX = 30f;

    void Update()
    {
        foreach (GameObject obj in objetosARotar)
        {
            // Rota el objeto en su propio eje local X
            obj.transform.Rotate(Vector3.right, velocidadRotacionX * Time.deltaTime, Space.Self);
        }
    }
}
