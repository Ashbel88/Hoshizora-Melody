using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    public static ScoreUI Instance;

    public TMP_Text scoreText;

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateScore(int newScore)
    {
        scoreText.text = newScore.ToString();
    }
}
