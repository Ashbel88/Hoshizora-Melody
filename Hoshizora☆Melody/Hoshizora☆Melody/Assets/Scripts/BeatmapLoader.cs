using UnityEngine;

public class BeatmapLoader : MonoBehaviour
{
    public BeatmapData Beatmap { get; private set; }

    public void SetBeatmapFromJson(string json)
    {
        if (string.IsNullOrEmpty(json))
        {
            Debug.LogError("BeatmapLoader: JSON string was null or empty!");
            return;
        }

        Beatmap = JsonUtility.FromJson<BeatmapData>(json);

        if (Beatmap == null || Beatmap.charts == null || Beatmap.charts.Length == 0)
        {
            Debug.LogError("BeatmapLoader: Failed to parse beatmap or no charts found.");
        }
        else
        {
            Debug.Log($"BeatmapLoader: Loaded beatmap via SetBeatmapFromJson. Charts={Beatmap.charts.Length}");
        }
    }

    public BeatmapData LoadBeatmap()
    {
        return Beatmap;
    }
}
