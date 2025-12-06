using System;
using UnityEngine;

public class HitManager : MonoBehaviour
{
    public static HitManager Instance;

    [Header("Timing Windows (ordered best -> worst)")]
    public TimingWindow[] timingWindows;

    [Header("Miss threshold (seconds after hitTime to auto-miss)")]
    public float missThreshold = 0.25f;

    private void Awake()
    {
        Instance = this;

        Array.Sort(timingWindows, (a, b) => a.maxTime.CompareTo(b.maxTime));
    }

    public string JudgeNote (Note note, float songTime, out TimingWindow matchedWindow)
    {
        matchedWindow = null;
        float diff = Mathf.Abs(note.hitTime - songTime);

        // iterate windows in order; the first that matches is used
        foreach (var w in timingWindows)
        {
            if (diff <= w.maxTime)
            {
                matchedWindow = w;
                return w.name;
            }
        }

        return null;
    }

    public bool IsMissed(Note note, float songTime)
    {
        return (songTime - note.hitTime) > missThreshold;
    }
}
