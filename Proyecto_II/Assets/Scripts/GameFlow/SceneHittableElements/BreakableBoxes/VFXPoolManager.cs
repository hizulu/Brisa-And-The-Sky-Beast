using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

// Jone Sainz Egea
// 29/03/2025
public class VFXPoolManager : MonoBehaviour
{
    [SerializeField] private VisualEffect smokePoofPrefab;
    [SerializeField] private int initialPoolSize = 5; // Tamaño inicial del pool

    private Queue<VisualEffect> pool = new Queue<VisualEffect>();
    public static VFXPoolManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        // Prellenar el pool con instancias
        for (int i = 0; i < initialPoolSize; i++)
        {
            VisualEffect vfx = Instantiate(smokePoofPrefab, transform);
            vfx.Stop();
            vfx.gameObject.SetActive(false);
            pool.Enqueue(vfx);
        }
    }

    public VisualEffect GetVFX()
    {
        if (pool.Count > 0)
        {
            VisualEffect vfx = pool.Dequeue();
            vfx.gameObject.SetActive(true);
            vfx.Reinit(); // Reinicia el VFX para evitar comportamientos extraños
            return vfx;
        }

        // Si el pool está vacío, instanciar uno nuevo
        VisualEffect newVFX = Instantiate(smokePoofPrefab, transform);
        newVFX.Reinit();
        return newVFX;
    }

    public void ReturnVFX(VisualEffect vfx)
    {
        vfx.Stop();
        vfx.gameObject.SetActive(false);
        pool.Enqueue(vfx);
    }
}
