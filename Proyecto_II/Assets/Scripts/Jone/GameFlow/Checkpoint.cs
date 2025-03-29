using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public bool Activated = false;

    public Material green;
    public Material magenta;

    public static List<GameObject> CheckPointsList;

    private SaveManager saveManager;

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

        GetComponent<MeshRenderer>().material = magenta;

        saveManager = SaveManager.instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ActivateCheckPoint();
            saveManager.SaveSceneState();
        }
    }

    private void ActivateCheckPoint()
    {
        // Deactivate all the checkpoints
        foreach (GameObject cp in CheckPointsList)
        {
            cp.GetComponent<Checkpoint>().Activated = false;
            cp.GetComponent<MeshRenderer>().material = magenta;
        }

        // Activate current checkpoint
        Activated = true;
        GetComponent<MeshRenderer>().material = green;
    }

    public static Vector3 GetActiveCheckPointPosition()
    {
        if (CheckPointsList == null || CheckPointsList.Count == 0)
            return Vector3.zero; // TODO: change for default position

        foreach (GameObject cp in CheckPointsList)
        {
            if (cp.GetComponent<Checkpoint>().Activated)
            {
                return cp.transform.position;
            }
        }

        return CheckPointsList[0].transform.position;
    }
}
