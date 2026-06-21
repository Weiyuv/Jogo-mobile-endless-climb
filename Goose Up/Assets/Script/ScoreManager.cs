using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    [Header("Score (run)")]
    public int score = 0;
    public TextMeshProUGUI scoreText;

    [Header("Coins (persistente)")]
    public int coins = 0;
    public TextMeshProUGUI coinsText;

    [Header("Upgrade UI")]
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI costText;
    public TextMeshProUGUI bonusText;

    public GameObject upgradePanel;

    [Header("Upgrade Data")]
    public int upgradeLevel = 1;
    public int upgradeCost = 10;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        upgradeLevel = PlayerPrefs.GetInt("UpgradeLevel", 1);
        upgradeCost = PlayerPrefs.GetInt("UpgradeCost", 10);

        coins = PlayerPrefs.GetInt("Coins", 0);

        score = 0;

        if (upgradePanel != null)
            upgradePanel.SetActive(false);

        UpdateUI();
    }

    public void AddScore(int amount)
    {
        score += amount * upgradeLevel;
        UpdateUI();
    }

    public void BankScore()
    {
        coins += score;

        PlayerPrefs.SetInt("Coins", coins);
        PlayerPrefs.Save();

        score = 0;

        UpdateUI();
    }

    public void BuyUpgrade()
    {
        if (coins < upgradeCost)
            return;

        coins -= upgradeCost;
        upgradeLevel++;
        upgradeCost += 5;

        PlayerPrefs.SetInt("Coins", coins);
        PlayerPrefs.SetInt("UpgradeLevel", upgradeLevel);
        PlayerPrefs.SetInt("UpgradeCost", upgradeCost);
        PlayerPrefs.Save();

        UpdateUI();
    }

    public void OpenUpgradePanel()
    {
        if (upgradePanel == null) return;

        upgradePanel.SetActive(true);
        UpdateUI();
    }

    public void CloseUpgradePanel()
    {
        if (upgradePanel == null) return;

        upgradePanel.SetActive(false);
    }

    void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = score.ToString();

        if (coinsText != null)
            coinsText.text = "Cultivation: " + coins;

        if (levelText != null)
            levelText.text = "Level: " + upgradeLevel;

        if (costText != null)
            costText.text = "Cost: " + upgradeCost;

        if (bonusText != null)
            bonusText.text = "+" + upgradeLevel + " score por plataforma";
    }
}