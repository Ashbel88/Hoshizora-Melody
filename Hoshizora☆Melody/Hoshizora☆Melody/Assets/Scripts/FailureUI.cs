using UnityEngine;

public class FailureUI : MonoBehaviour
{
    public static FailureUI Instance;

    public GameObject failScreen;

    void Awake()
    {
        Instance = this;
        failScreen.SetActive(false);
    }

    public void ShowFailure()
    {
        failScreen.SetActive(true);
    }
}
