using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneTap : MonoBehaviour
{
    public string songSelectScene = "SongSelectScene";
    public float inputDelay = 0.2f; // delay before taps count

    private bool inputAllowed = false;

    void Start()
    {
        StartCoroutine(EnableInputAfterDelay());
    }

    IEnumerator EnableInputAfterDelay()
    {
        yield return new WaitForSeconds(inputDelay);
        inputAllowed = true;
    }

    void Update()
    {
        if (!inputAllowed)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            LoadNextScene();
            inputAllowed = false;
        }
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(songSelectScene);
    }
}
