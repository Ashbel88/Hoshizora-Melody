using System.Collections.Generic;
using System.Runtime.InteropServices;
using NUnit.Framework;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    public static NoteManager Instance;

    private readonly List<Note> activeNotes = new List<Note>();

    private void Awake()
    {
        Instance = this;
    }

    public void RegisterNote(Note note)
    {
        if (!activeNotes.Contains(note))
        {
            activeNotes.Add(note);
        }
    }

    public void RemoveNote(Note note)
    {
        if (activeNotes.Contains(note))
        {
            activeNotes.Remove(note);
        }

        if (note != null && note.gameObject != null)
        {
            Destroy(note.gameObject);
        }
    }

    // Return the closest note in the given lane by absolute time difference to current song time
    public Note GetClosestNoteInLane(int laneIndex, float songTime, float maxJudgeWidow)
    {
        Note best = null;
        float bestTime = float.MaxValue;

        for(int i = 0; i < activeNotes.Count; i++)
        {
            var n = activeNotes[i];
            if (n == null) continue;
            if (n.laneIndex != laneIndex) continue;
            if (n.isHit) continue;

            float timeDifference = n.hitTime - songTime;

            // Skip notes that are too early to judge

            if (timeDifference < -maxJudgeWidow)
                continue;

            // Skip notes too far in the future (not yet in range)

            if (timeDifference > maxJudgeWidow)
                continue;

            // Choose the earliest note that is in the judge range
            if (Mathf.Abs(timeDifference) < bestTime)
            {
                bestTime = Mathf.Abs(timeDifference);
                best = n;
            }

        }

        return best;
    }
}
