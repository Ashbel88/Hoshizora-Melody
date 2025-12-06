using TMPro;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    public static HealthUI Instance;

    public TMP_Text healthText;

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateHealth(float health)
    {
        healthText.text = Mathf.RoundToInt(health).ToString();
    }
}
