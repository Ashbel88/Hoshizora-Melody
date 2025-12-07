using System;
using System.Collections;
using UnityEngine;

public class BeatmapPlayer : MonoBehaviour
{
    [Header("References")]
    public BeatmapLoader beatmapLoader;
    public NoteSpawner noteSpawner;
    public AudioManager audioManager;

    [Header("Chart selection")]
    public int chartIndex = 0;

    [Header("Spawn Lead Time (seconds)")]
    public float spawnLeadTime = 1.2f;

    private BeatmapData beatmap;
    private ChartData activeChart;
    private NoteData[] notes;

    private int nextNoteIndex = 0;

    private const float DOUBLE_WINDOW_MS = 0.5f; // notes ≤ 0.5ms apart = double note

    public static BeatmapPlayer Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void BeginPlayback()
    {
        nextNoteIndex = 0;
        enabled = true;
    }

    public void StopPlayback()
    {
        enabled = false;
        nextNoteIndex = 0;
    }

    public void StartPlaybackWhenReady()
    {
        StartCoroutine(WaitForSongStart());
    }

    private IEnumerator WaitForSongStart()
    {
        while (audioManager.song.time > 0.01f)
            yield return null;

        yield return new WaitForEndOfFrame();

        nextNoteIndex = 0;
        enabled = true;

        Debug.Log("BeatmapPlayer started playback");
    }

    public void SetBeatmapAndStart(BeatmapData loadedBeatmap)
    {

        if (loadedBeatmap == null)
        {
            Debug.LogError("BeatmapPlayer: SetBeatmapAndStart received NULL beatmap!");
            return;
        }

        beatmap = loadedBeatmap;

        if (beatmap.charts == null || beatmap.charts.Length == 0)
        {
            Debug.LogError("BeatmapPlayer: Beatmap has no charts.");
            return;
        }

        
        activeChart = beatmap.charts[0];

        if (activeChart.notes == null || activeChart.notes.Length == 0)
        {
            Debug.LogError("BeatmapPlayer: Chart has no notes.");
            return;
        }

        notes = activeChart.notes;
        Array.Sort(notes, (a, b) => a.songPos.CompareTo(b.songPos));

        nextNoteIndex = 0;

        Debug.Log($"BeatmapPlayer: Beatmap ready — {notes.Length} notes loaded.");
    }

    void Update()
    {
        if (notes == null || audioManager == null || noteSpawner == null)
            return;

        float songTimeSec = audioManager.GetSongTime();
        float spawnCutoffMs = (songTimeSec + spawnLeadTime) * 1000f;

        while (nextNoteIndex < notes.Length && notes[nextNoteIndex].songPos <= spawnCutoffMs)
        {
            HandleNoteSpawning();
        }
    }


    // NOTE SPAWNING WITH DOUBLE-NOTE DETECTION
 
    void HandleNoteSpawning()
    {
        NoteData n = notes[nextNoteIndex];
        float hitTimeSec = n.songPos / 1000f;

        // Check if there is a second note at the exact same time
        if (nextNoteIndex + 1 < notes.Length)
        {
            NoteData next = notes[nextNoteIndex + 1];

            if (Mathf.Abs(next.songPos - n.songPos) <= DOUBLE_WINDOW_MS)
            {
                noteSpawner.SpawnDoubleNote(n.lane, next.lane, hitTimeSec);

                nextNoteIndex += 2; // Skip BOTH notes
                return;
            }
        }

        // Otherwise: normal single tap
        if (n.lane >= 0 && n.lane < noteSpawner.tapPoints.Length)
        {
            noteSpawner.SpawnNote(n.lane, hitTimeSec);
        }
        else
        {
            Debug.LogWarning($"BeatmapPlayer: Note lane {n.lane} is out of range, skipping.");
        }

        nextNoteIndex++;
    }
}
