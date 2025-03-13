using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject opcionesMenu;

    private void Start()
    {
        mainMenu.SetActive(true);
        opcionesMenu.SetActive(false);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }

    public void Opciones()
    {
        mainMenu.SetActive(false);
        opcionesMenu.SetActive(true);
    }

    public void Volver()
    {
        mainMenu.SetActive(true);
        opcionesMenu.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Has salido del juego");
    }
}
