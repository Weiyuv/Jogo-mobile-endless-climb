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

        if (deathUI != null)
            deathUI.SetActive(false);
    }

    public void PlayerDied()
    {
        // converte score da run em coins
        if (ScoreManager.instance != null)
            ScoreManager.instance.BankScore();

        Time.timeScale = 0f;

        if (deathUI != null)
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