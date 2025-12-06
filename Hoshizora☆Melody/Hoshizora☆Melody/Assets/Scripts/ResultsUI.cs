using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultsUI : MonoBehaviour
{
    public static ResultsUI Instance;

    [Header("Panels")]
    [SerializeField] GameObject resultsPanel;

    [Header("Judgement Text")]
    [SerializeField] TMP_Text perfectText;
    [SerializeField] TMP_Text greatText;
    [SerializeField] TMP_Text goodText;
    [SerializeField] TMP_Text badText;
    [SerializeField] TMP_Text missText;

    [Header("Score + Accuracy")]
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text accuracyText;

    [Header("Combo")]
    [SerializeField] TMP_Text highestComboText;
    [SerializeField] GameObject fullComboUI;

    [Header("Rank")]
    [SerializeField] Image rankingImage;
    [SerializeField] Sprite rankS;
    [SerializeField] Sprite rankA;
    [SerializeField] Sprite rankB;
    [SerializeField] Sprite rankC;

    private void Awake()
    {
        Instance = this;
        resultsPanel.SetActive(false);
    }

    public void ShowResults()
    {
        resultsPanel.SetActive(true);

        var PM = PlayerManager.Instance;

        // --- Judgement numbers ---
        perfectText.text = PM.perfectCount.ToString();
        greatText.text = PM.greatCount.ToString();
        goodText.text = PM.goodCount.ToString();
        badText.text = PM.badCount.ToString();
        missText.text = PM.missCount.ToString();

        // --- Score ---
        scoreText.text = PM.score.ToString("N0");

        // --- Accuracy ---
        float acc = PM.GetAccuracy();
        accuracyText.text = acc.ToString("F2") + "%";

        // --- Combo ---
        highestComboText.text = PM.MaxCombo.ToString();

        // Full combo = no bads & no misses
        bool fullCombo = (PM.badCount == 0 && PM.missCount == 0);
        fullComboUI.SetActive(fullCombo);

        // Rank calculation
        rankingImage.sprite = GetRankSprite(acc);
    }

    private Sprite GetRankSprite(float acc)
    {
        if (acc >= 98f) return rankS;
        if (acc >= 90f) return rankA;
        if (acc >= 80f) return rankB;
        return rankC;
    }
}
