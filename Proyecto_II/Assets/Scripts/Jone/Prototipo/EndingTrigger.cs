using System.Collections;
using System.Collections.Generic;
//using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndingTrigger : MonoBehaviour
{
    public static bool beastUp;
    [SerializeField] private GameObject defaultPanel; // Panel de interacción
    // [SerializeField] private TextMeshProUGUI panelText; // Texto dentro del panel

    // [SerializeField] private string panelMessage = "Necesitas que la bestia suba a la plataforma para continuar.";

    private void Start()
    {
        defaultPanel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
       if(other.CompareTag("Player"))
       {
            if (!beastUp)
            {
                defaultPanel.SetActive(true);
                //panelText.text = panelMessage;
            }
            else
            {
                SceneManager.LoadScene(0);
            }
       } 
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            defaultPanel.SetActive(false);
        }
    }
}
