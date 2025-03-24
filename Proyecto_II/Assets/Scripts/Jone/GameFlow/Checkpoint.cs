using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public bool Activated = false;

    private SpriteRenderer thisSpriteRenderer;

    public static List<GameObject> CheckPointsList;

    void Start()
    {
        if (CheckPointsList == null)
        {
            CheckPointsList = new List<GameObject>();
        }

        if (!CheckPointsList.Contains(gameObject))
        {
            CheckPointsList.Add(gameObject);
        }

        thisSpriteRenderer = GetComponent<SpriteRenderer>();
        thisSpriteRenderer.color = Color.magenta;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ActivateCheckPoint();
        }
    }

    private void ActivateCheckPoint()
    {
        foreach (GameObject cp in CheckPointsList)
        {
            cp.GetComponent<Checkpoint>().Activated = false;
            cp.GetComponent<SpriteRenderer>().color = Color.magenta;
        }

        Activated = true;
        thisSpriteRenderer.color = Color.green;
    }

    public static Vector3 GetActiveCheckPointPosition()
    {
        Vector3 result = new Vector3(0, -9.5f, -5f);

        if (CheckPointsList != null)
        {
            foreach (GameObject cp in CheckPointsList)
            {
                if (cp.GetComponent<Checkpoint>().Activated)
                {
                    result = cp.transform.position;
                    break;
                }
            }
        }

        return result;
    }
}
