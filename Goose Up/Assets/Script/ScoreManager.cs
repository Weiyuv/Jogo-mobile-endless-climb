using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    [Header("Score")]
    public int score = 0;
    public TextMeshProUGUI scoreText;

    [Header("Upgrade")]
    public GameObject upgradePanel;
    public TextMeshProUGUI upgradeText;

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
        if (upgradePanel != null)
            upgradePanel.SetActive(false);

        UpdateUI();
    }

    public void AddScore(int amount)
    {
        score += amount * upgradeLevel;
        UpdateUI();
    }

    public void BuyUpgrade()
    {
        if (score < upgradeCost) return;

        score -= upgradeCost;
        upgradeLevel++;
        upgradeCost += 5;

        UpdateUI();
    }

    // ✅ ABRIR PANEL
    public void OpenUpgradePanel()
    {
        if (upgradePanel == null) return;

        upgradePanel.SetActive(true);
        UpdateUI();
    }

    // ✅ FECHAR PANEL
    public void CloseUpgradePanel()
    {
        if (upgradePanel == null) return;

        upgradePanel.SetActive(false);
    }

    void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = score.ToString();

        if (upgradeText != null)
        {
            upgradeText.text =
                "Level: " + upgradeLevel +
                "\nScore: " + score +
                "\nCost: " + upgradeCost;
        }
    }
}