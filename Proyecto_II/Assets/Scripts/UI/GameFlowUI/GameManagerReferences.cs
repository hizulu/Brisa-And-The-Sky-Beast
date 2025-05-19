// Jone Sainz Egea
// 19/05/2025
using UnityEngine;

public class GameManagerReferences : MonoBehaviour
{
    public void ResumeGame()
    {
        GameManager.Instance.ResumeGame();
    }

    public void BackToMainMenu()
    {
        GameManager.Instance.BackToMainMenu();
    }

    public void ReloadScene()
    {
        GameManager.Instance.ReloadScene();
    }
}
