using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirarAspasMolinosRotos : MonoBehaviour
{
    public GameObject[] objetosARotar;
    public float anguloTiron = 15f;                // �ngulo m�ximo del tir�n a cada lado
    public float tironDuracion = 0.2f;             // Duraci�n total del tir�n (ida y vuelta)
    public float tiempoEntreTironesMin = 1f;
    public float tiempoEntreTironesMax = 3f;

    private float[] tiempoSiguienteTiron;
    private float[] tironActivoHasta;
    private bool[] enTiron;

    void Start()
    {
        int n = objetosARotar.Length;
        tiempoSiguienteTiron = new float[n];
        tironActivoHasta = new float[n];
        enTiron = new bool[n];

        for (int i = 0; i < n; i++)
        {
            tiempoSiguienteTiron[i] = Time.time + Random.Range(tiempoEntreTironesMin, tiempoEntreTironesMax);
            tironActivoHasta[i] = 0f;
            enTiron[i] = false;
        }
    }

    void Update()
    {
        for (int i = 0; i < objetosARotar.Length; i++)
        {
            if (!enTiron[i] && Time.time >= tiempoSiguienteTiron[i])
            {
                // Inicia el tir�n
                enTiron[i] = true;
                tironActivoHasta[i] = Time.time + tironDuracion;
                tiempoSiguienteTiron[i] = Time.time + Random.Range(tiempoEntreTironesMin, tiempoEntreTironesMax);
            }

            if (enTiron[i])
            {
                float t = (Time.time - (tironActivoHasta[i] - tironDuracion)) / tironDuracion;
                float angulo = Mathf.Sin(t * Mathf.PI) * anguloTiron;

                // Obtener la rotaci�n actual del objeto
                Vector3 rotActual = objetosARotar[i].transform.localEulerAngles;

                // Modificar solo el eje X, dejando Y y Z intactos
                rotActual.x = angulo;

                // Aplicar la nueva rotaci�n solo en X
                objetosARotar[i].transform.localEulerAngles = rotActual;

                if (Time.time >= tironActivoHasta[i])
                {
                    enTiron[i] = false;
                    // Reset al centro en X (sin afectar Y y Z)
                    rotActual.x = 0f;
                    objetosARotar[i].transform.localEulerAngles = rotActual;
                }
            }
        }
    }
}

