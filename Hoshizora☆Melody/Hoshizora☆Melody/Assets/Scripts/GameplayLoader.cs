using System.IO;
using UnityEngine;
using UnityEngine.LightTransport;
using UnityEngine.Video;

public class GameplayLoader : MonoBehaviour
{
    [Header("Components")]
    public AudioManager audioManager;
    public VideoPlayer videoPlayer;
    public BeatmapLoader beatmapLoader;
    public BeatmapPlayer beatmapPlayer;

    private string songID;

    void Start()
    {
        songID = GameSession.selectSongID;

        LoadAudio();
        LoadVideo();
        LoadBeatmap();

        StartCoroutine(StartWhenReady());
    }

    void LoadAudio()
    {
        string path = $"Music Audio/{songID} (Audio)";
        AudioClip song = Resources.Load<AudioClip>(path);

        if (song == null)
        {
            Debug.LogError($"Audio not found for song: {songID}");
            return;
        }

        audioManager.SetSong(song);
    }

    void LoadVideo()
    {
        string fileName = $"{songID} (Video).mp4";
        string path = Path.Combine(Application.streamingAssetsPath, "Music Videos", fileName);

        if (!File.Exists(path))
        {
            Debug.LogWarning($"No MV found for {songID}, skipping video.");
            videoPlayer.gameObject.SetActive(false);
            return;
        }

        videoPlayer.url = path;
        videoPlayer.Prepare();
    }

    void LoadBeatmap()
    {
        string fileName = $"{songID} (Beatmap).json";

        string path = Path.Combine(Application.streamingAssetsPath, "Beatmaps", fileName);

        if (!File.Exists(path))
        {
            Debug.LogError($"Beatmap JSON missing for: {songID}");
            return;
        }

        string json = File.ReadAllText(path);
        BeatmapData map = JsonUtility.FromJson<BeatmapData>(json);
        beatmapPlayer.SetBeatmapAndStart(map);
        
    }

    System.Collections.IEnumerator StartWhenReady()
    {
        // Wait for video to be ready
        if (videoPlayer.gameObject.activeSelf)
        {
            while (!videoPlayer.isPrepared)
                yield return null;
        }

        beatmapPlayer.StartPlaybackWhenReady();
        audioManager.Play();
        videoPlayer.Play();
    }
}
