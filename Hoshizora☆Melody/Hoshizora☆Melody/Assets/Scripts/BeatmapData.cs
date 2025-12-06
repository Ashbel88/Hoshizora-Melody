using System;
using UnityEngine;

[Serializable]
public class BeatmapData
{
    public float bpm;
    public float offset;
    public ChartData[] charts;
}

[Serializable]
public class ChartData
{
    public string uuid;
    public NoteData[] notes;
}

[Serializable]
public class NoteData
{
    public string uuid;
    public float songPos;
    public float beat;
    public int color;
    public string label;
    public int lane;      
}
