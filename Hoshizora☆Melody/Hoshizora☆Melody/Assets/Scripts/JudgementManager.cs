using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class JudgementManager : MonoBehaviour
{
    public static JudgementManager Instance;

    [Header("UI")]
    public Image judgeImage;
    public float showTime = 0.4f;
    public float scaleAmount = 1.2f;

    [Header("Sprites")]
    [SerializeField] Sprite perfectSprite;
    [SerializeField] Sprite greatSprite;
    [SerializeField] Sprite goodSprite;
    [SerializeField] Sprite badSprite;
    [SerializeField] Sprite missSprite;

    private void Awake()
    {
        Instance = this;
        judgeImage.enabled = false;
    }

    public void ShowJudgement(string type)
    {
        type = type.ToUpper();
        switch (type)
        {
            case "PERFECT": judgeImage.sprite = perfectSprite;
                break;
            case "GREAT": judgeImage.sprite = greatSprite;
                break;
            case "GOOD": judgeImage.sprite = goodSprite;
                break;
            case "BAD": judgeImage.sprite = badSprite;
                break;
            case "MISS": judgeImage.sprite = missSprite;
                break;
        }

        judgeImage.SetNativeSize();
        judgeImage.rectTransform.sizeDelta *= 2;

        StopAllCoroutines();
        StartCoroutine(PlayPopup());
    }

    IEnumerator PlayPopup()
    {
        judgeImage.enabled = true;

        // Resetting

        judgeImage.transform.localScale = Vector3.zero;
        float timer = 0f;

        // Scale in

        while (timer < 0.1f)
        {
            timer += Time.deltaTime;
            judgeImage.transform.localScale =
                Vector3.Lerp(Vector3.zero, Vector3.one * scaleAmount, timer / 0.1f);
            yield return null;
        }

        yield return new WaitForSeconds(showTime);

        // Hide

        judgeImage.enabled = false;

    }
}
