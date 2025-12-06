using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    [Header("Enter name of the Next Scene")]
    public string NextScene;

    public void LoadNextScene()
    {
        SceneManager.LoadScene(NextScene);
    }
}
