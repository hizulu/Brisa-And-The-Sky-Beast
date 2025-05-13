using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject creditosMenu;

    private void Start()
    {
        mainMenu.SetActive(true);
        creditosMenu.SetActive(false);
    }

    public void PlayGame()
    {
        EventsManager.CleanAllEvents();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Has salido del juego");
    }
}
