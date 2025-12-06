using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioSource song;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (song != null && song.isPlaying)
        {
            // Song finished
            if (song.time >= song.clip.length - 0.05f)
            {
                song.Stop();
                ResultsUI.Instance.ShowResults();
            }
        }
    }

    public float GetSongTime()
    {
        if (song == null || song.clip == null)
            return 0f;

        return song.time;
    }

    public void SetSong(AudioClip clip)
    {
        song.clip = clip;
    }

    public void Play()
    {
        if (song.clip != null)
            song.Play();
    }

    public void Stop()
    {
        song.Stop();
    }
}
