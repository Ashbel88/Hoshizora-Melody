using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    [Header("Score")]
    public int score = 0;
    public int combo = 0;
    public int MaxCombo = 0;

    [Header("Accuracy Tracking")]
    public int totalNotes = 0;
    public int perfectCount = 0;
    public int greatCount = 0;
    public int goodCount = 0;
    public int badCount = 0;
    public int missCount = 0;

    [Header("Health")]
    public float maxHealth = 100f;
    public float health = 100f;

    [Header("Health Values")]
    public float perfectHeal = 0.5f;
    public float greatHeal = 0.3f;
    public float goodHeal = 0.1f;
    public float badDmg = 3f;
    public float missDmg = 7f;

    private void Awake()
    {
        Instance = this;
    }

    
    void Start()
    {
        ResetStats();
        UpdateHealth(maxHealth);
        UpdateScore(score);
    }

    void Update()
    {
        UpdateCombo(combo);
    }

    public void ApplyJudgement(string judgement, TimingWindow window)
    {
        totalNotes++;
        judgement = judgement.ToUpper();

        switch (judgement)
        {
            case "PERFECT":
                perfectCount++;
                combo++;
                Heal(perfectHeal);
                score += window.scoreValue;
                UpdateScore(score);
                break;

            case "GREAT":
                greatCount++;
                combo++;
                Heal(greatHeal);
                score += window.scoreValue;
                UpdateScore(score);
                break;

            case "GOOD":
                goodCount++;
                combo++;
                Heal(goodHeal);
                score += window.scoreValue;
                UpdateScore(score);
                break;

            case "BAD":
                badCount++;
                combo = 0;  
                Damage(badDmg);
                break;

            case "MISS":
                missCount++;
                combo = 0;
                Damage(missDmg);
                break;
        }

        Debug.Log($"Judgement {judgement}, window={(window != null ? window.name : "NULL")}, addScore={(window != null ? window.scoreValue : 0)}, newScore={score}");
    }


    public void Heal(float amount)
    {
        health = Mathf.Clamp(health + amount, 0, maxHealth);
        UpdateHealth(health);
    }

    public void Damage(float amount)
    {
        health = Mathf.Clamp(health - amount, 0, maxHealth);
        UpdateHealth(health);
        if (health <= 0)
        {
            FailureUI.Instance.ShowFailure();
            BeatmapPlayer.Instance.StopPlayback();
            AudioManager.Instance.Stop();
        }
    }

    public float GetAccuracy()
    {
        if (totalNotes == 0) return 0f;

        float weightedScore =
            perfectCount * 1.0f +
            greatCount * 0.9f +
            goodCount * 0.75f +
            badCount * 0.3f;

        return (weightedScore / totalNotes) * 100f;
    }

    private void UpdateScore(int score)
    {
        ScoreUI.Instance.UpdateScore(score);
    }

    private void UpdateHealth(float health)
    {
        HealthUI.Instance.UpdateHealth(health);
    }

    private void UpdateCombo(int combo)
    {
        ComboUI.Instance.UpdateCombo(combo);
    }

    private void ResetStats() 
    {
        score = 0;
        combo = 0;
        MaxCombo = 0;
        totalNotes = 0;
        perfectCount = 0;
        greatCount = 0;
        goodCount = 0;
        badCount = 0;
        missCount = 0;
}
}
