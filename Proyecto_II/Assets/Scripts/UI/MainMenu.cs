using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static GameManager;

// Lucía García López, Jone Sainz Egea
public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject creditosMenu;
    [SerializeField] private GameObject seguroMenu;
    [SerializeField] private GameObject botonCargarPartida;

    private void Start()
    {
        mainMenu.SetActive(true);
        creditosMenu.SetActive(false);

        if (GameManager.HasSaved)
        {
            botonCargarPartida.SetActive(true);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (creditosMenu.activeSelf || seguroMenu.activeSelf)
            {
                Volver();
            }
            else
            {
                EstasSeguro();
            }
        }
    }

    public void PlayGame()
    {
        EventsManager.CleanAllEvents();

        GameSession.IsNewGame = true;

        GameManager.Instance.LoadScene("01_OpeningCinematic");
    }

    public void ContinueGame()
    {
        GameSession.IsNewGame = false;
        GameManager.Instance.LoadScene("02_TheHollow"); //TODO: guardar nombre de escena
    }

    public void Creditos()
    {
        mainMenu.SetActive(false);
        creditosMenu.SetActive(true);
    }

    public void Volver()
    {
        mainMenu.SetActive(true);
        creditosMenu.SetActive(false);
        seguroMenu.SetActive(false);
    }

    public void EstasSeguro()
    {
        seguroMenu.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Has salido del juego");
    }
}
