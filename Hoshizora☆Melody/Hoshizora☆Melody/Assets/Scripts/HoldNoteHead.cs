using UnityEngine;

public class HoldNoteHead : MonoBehaviour
{
    // WORK IN PROGRESS

    [Header("Hold Settings")]
    public float hitTime;        // when the head should be tapped
    public float holdLength;     // how long the player must hold
    public int laneIndex;

    [Header("Prefab References")]
    public GameObject holdUIPrefab;

    private Transform tapPoint;
    private bool activated = false;

    public void Initialize(float hitTime, float holdLength, int lane, Transform tapPoint)
    {
        this.hitTime = hitTime;
        this.holdLength = holdLength;
        this.laneIndex = lane;
        this.tapPoint = tapPoint;

        Note note = GetComponent<Note>();
        if (note != null)
        {
            note.hitTime = hitTime;
            note.laneIndex = lane;
        }

        NotePath path = GetComponent<NotePath>();
        if (path != null)
            path.autoDelete = false;
    }

    public void OnHeadHit()
    {
        if (activated) return;
        activated = true;

        // Spawn hold UI at tap point (stationary)
        GameObject uiObj = Instantiate(holdUIPrefab, tapPoint.position, Quaternion.identity);

        HoldNoteUI ui = uiObj.GetComponent<HoldNoteUI>();
        if (ui != null)
        {
            ui.Initialize(hitTime, holdLength);
        }
    }

    public void OnHeadMiss()
    {
        // Entire hold note is canceled
        Destroy(gameObject);
    }
}