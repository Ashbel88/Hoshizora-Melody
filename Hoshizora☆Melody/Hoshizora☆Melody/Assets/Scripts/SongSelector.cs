using UnityEngine;
using UnityEngine.SceneManagement;

public class SongSelector : MonoBehaviour
{
    [Header("Set name of Audio File")]
    public string songID;
    [Header("Set name of Gameplay Scene")]
    public string gameScene = "";

    public void OnSongButtonPressed()
    {
        GameSession.selectSongID = songID;
        SceneManager.LoadScene(gameScene);

    }
}
