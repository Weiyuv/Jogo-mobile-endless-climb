using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject deathUI;

    void Awake()
    {
        instance = this;
        Time.timeScale = 1f;
        deathUI.SetActive(false);
    }

    public void PlayerDied()
    {
        Time.timeScale = 0f;
        deathUI.SetActive(true);
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }
}