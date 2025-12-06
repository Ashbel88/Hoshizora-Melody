using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneTap : MonoBehaviour
{
    public string songSelectScene = "SongSelectScene";

    // Update is called once per frame
    void Update()
    {if (Input.GetMouseButtonDown(0))
            LoadNextScene();
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(songSelectScene);
    }
}
