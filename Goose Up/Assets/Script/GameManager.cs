using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject deathUI;

    [Header("Platform UI")]
    public GameObject platformButton;
    public Image platformButtonImage;
    public Color platformReadyColor = Color.green;
    public Color platformNotReadyColor = Color.gray;

    [Header("Double Tap UI")]
    public GameObject doubleTapButton;
    public Image doubleTapButtonImage;
    public Color doubleTapReadyColor = Color.green;
    public Color doubleTapNotReadyColor = Color.gray;

    void Awake()
    {
        instance = this;
        Time.timeScale = 1f;

        if (deathUI != null)
            deathUI.SetActive(false);

        if (platformButton != null)
            platformButton.SetActive(true);

        if (doubleTapButton != null)
            doubleTapButton.SetActive(true);
    }

    public void PlayerDied()
    {
        if (ScoreManager.instance != null)
            ScoreManager.instance.BankScore();

        Time.timeScale = 0f;

        if (deathUI != null)
            deathUI.SetActive(true);

        if (platformButton != null)
            platformButton.SetActive(false);

        if (doubleTapButton != null)
            doubleTapButton.SetActive(false);
    }

    public void SetPlatformReady(bool ready)
    {
        if (platformButtonImage != null)
            platformButtonImage.color = ready ? platformReadyColor : platformNotReadyColor;
    }

    public void SetDoubleTapReady(bool ready)
    {
        if (doubleTapButtonImage != null)
            doubleTapButtonImage.color = ready ? doubleTapReadyColor : doubleTapNotReadyColor;
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