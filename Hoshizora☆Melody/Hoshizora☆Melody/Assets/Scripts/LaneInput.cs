using UnityEngine;

public class LaneInput : MonoBehaviour
{
    [Header("Keys for each of the 7 lanes")]
    public KeyCode[] laneKeys = new KeyCode[7]
    {
        KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.F,
        KeyCode.J, KeyCode.K, KeyCode.L
    };

    [Header("How far away (sec) we search for notes")]
    public float searchWindow = 1f;

    private HoldNoteUI[] activeHoldUIs = new HoldNoteUI[7];
    private bool[] isHolding = new bool[7];

    void Update()
    {
        CheckKeyDown();
        CheckKeyUp();
    }

  
    //  ON PRESS
 
    void CheckKeyDown()
    {
        for (int lane = 0; lane < laneKeys.Length; lane++)
        {
            if (Input.GetKeyDown(laneKeys[lane]))
            {
                TryHitNote(lane);
            }
        }
    }

    //  ON RELEASE (For hold notes)
    void CheckKeyUp()
    {
        float songTime = AudioManager.Instance.GetSongTime();

        for (int lane = 0; lane < laneKeys.Length; lane++)
        {
            if (Input.GetKeyUp(laneKeys[lane]))
            {
                // Player let go — release hold note if active
                if (activeHoldUIs[lane] != null)
                {
                    activeHoldUIs[lane].OnRelease(songTime);
                    activeHoldUIs[lane] = null;
                    isHolding[lane] = false;
                }
            }
        }
    }

    
    //  HIT NOTE (normal or hold head)

    void TryHitNote(int lane)
    {
        float songTime = AudioManager.Instance.GetSongTime();

        Note note = NoteManager.Instance.GetClosestNoteInLane(lane, songTime, searchWindow);

        if (note == null)
            return;

        // Check if head note
        HoldNoteHead holdHead = note.GetComponent<HoldNoteHead>();

        // Handle normal tap notes
        if (holdHead == null)
        {
            HandleNormalNoteHit(note, songTime, lane);
            return;
        }

        // Handle hold note HEAD
        HandleHoldHeadHit(holdHead, songTime, lane);
    }

 
    //  NORMAL TAP NOTES

    void HandleNormalNoteHit(Note note, float songTime, int lane)
    {
        TimingWindow matched;
        string result = HitManager.Instance.JudgeNote(note, songTime, out matched);

        if (result != null)
        {
            PlayerManager.Instance.ApplyJudgement(result, matched);
            HitSFXManager.Instance.PlaySFX(result);
            note.OnHit(matched);
            NoteManager.Instance.RemoveNote(note);
            JudgementManager.Instance.ShowJudgement(result);
            
        }
    }

 
    //  HOLD NOTE HEAD (Work in Progress)

    void HandleHoldHeadHit(HoldNoteHead head, float songTime, int lane)
    {
        TimingWindow matched;
        string result = HitManager.Instance.JudgeNote(head.GetComponent<Note>(), songTime, out matched);

        if (result == null)
            return; // too early or late

        PlayerManager.Instance.ApplyJudgement(result, matched);

        // Sound
        HitSFXManager.Instance.PlaySFX(result);

        // Show head judgement
        JudgementManager.Instance.ShowJudgement(result);

        // Notify Head
        head.OnHeadHit();

        // Tell NoteManager to remove the head object
        NoteManager.Instance.RemoveNote(head.GetComponent<Note>());

        // Look for the Hold UI we just spawned
        activeHoldUIs[lane] = FindObjectOfType<HoldNoteUI>();

        // Mark as being held
        isHolding[lane] = true;
    }
}