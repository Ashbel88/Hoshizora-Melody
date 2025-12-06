using UnityEngine;

public class Note : MonoBehaviour
{
    [Header("Gameplay")]
    public int laneIndex;
    public float hitTime;
    public bool isHit = false;

    void Start()
    {
        NoteManager.Instance.RegisterNote(this);
    }


    void Update()
    {
        float songTime = AudioManager.Instance.GetSongTime();

        // auto miss if overdue
        if (!isHit && HitManager.Instance != null && HitManager.Instance.IsMissed(this, songTime))
        {
            NoteManager.Instance.RemoveNote(this);
            Debug.Log($"Missed note lane {laneIndex} at {songTime:F3}s (hitTime {hitTime:F3}s)");
            JudgementManager.Instance.ShowJudgement("MISS");
            PlayerManager.Instance.ApplyJudgement("MISS", null);
        }
    }

    public virtual void OnHit(TimingWindow window)
    {
        isHit = true;
    }
}
