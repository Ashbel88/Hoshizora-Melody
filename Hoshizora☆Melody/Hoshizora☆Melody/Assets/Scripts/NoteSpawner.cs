using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    [Header("Song Audio Source")]
    public AudioSource songAudioSource;

    [Header("Note Prefabs")]
    [SerializeField] GameObject notePrefab;
    [SerializeField] GameObject doubleNotePrefab;
    [SerializeField] GameObject holdHeadPrefab;

    [Header("Spawn Origin")]
    [SerializeField] Transform spawnOrigin;

    [Header("Line Tap Points")]
    public Transform[] tapPoints = new Transform[7];

    [Header("Off-Screen End Point")]
    public float endPointDistance = 2f;

    [Header("Arc Heights Per Lane")]
    public float[] laneHeights = { 1.4f, 1.1f, 0.9f, 0.7f, 0.9f, 1.1f, 1.4f };

    [Header("Timing")]
    public float travelTime = 1.2f;

    [Header("Note Size")]
    public Vector3 noteMaxScale;

    void Update()
    {
        float currentTime = songAudioSource.time;
    }

    // SPAWN NORMAL NOTE

    public GameObject SpawnNote(int laneIndex, float hitTime, GameObject overridePrefab = null)
    {
        if (laneIndex < 0 || laneIndex >= tapPoints.Length)
        {
            Debug.LogError("Invalid lane index: " + laneIndex);
            return null;
        }

        GameObject prefab = overridePrefab == null ? notePrefab : overridePrefab;

        GameObject noteObj = Instantiate(prefab);
        noteObj.transform.position = spawnOrigin.position;
        noteObj.transform.rotation = Quaternion.identity;
        noteObj.transform.localScale = notePrefab.transform.localScale;

        // Assign Note values
        Note note = noteObj.GetComponent<Note>();
        note.laneIndex = laneIndex;
        note.hitTime = hitTime;

        // Assign path data
        NotePath path = noteObj.GetComponent<NotePath>();
        path.startPoint = spawnOrigin.position;
        path.tapPoint = tapPoints[laneIndex].position;

        Vector3 direction = (tapPoints[laneIndex].position - spawnOrigin.position).normalized;
        path.endPoint = tapPoints[laneIndex].position + direction * endPointDistance;

        path.controlHeight = laneHeights[laneIndex];
        path.travelTime = travelTime;
        path.maxScale = notePrefab.transform.localScale;
        path.timer = 0f;

        return noteObj;
    }


    // DOUBLE NOTE

    public void SpawnDoubleNote(int laneA, int laneB, float hitTime)
    {
        SpawnNote(laneA, hitTime, doubleNotePrefab);
        SpawnNote(laneB, hitTime, doubleNotePrefab);
    }



    // HOLD NOTE (Work in Progess)

    public void SpawnHoldNote(int laneIndex, float holdLength)
    {
        GameObject headObj = Instantiate(holdHeadPrefab);
        headObj.transform.position = spawnOrigin.position;

        NotePath path = headObj.GetComponent<NotePath>();
        path.startPoint = spawnOrigin.position;
        path.tapPoint = tapPoints[laneIndex].position;

        Vector3 dir = (tapPoints[laneIndex].position - spawnOrigin.position).normalized;
        path.endPoint = tapPoints[laneIndex].position;
        path.controlHeight = laneHeights[laneIndex];
        path.travelTime = travelTime;
        path.maxScale = notePrefab.transform.localScale;
        path.timer = 0f;

        // CALCULATE true hit time
        float songTime = AudioManager.Instance.GetSongTime();
        float hitTime = songTime + travelTime;

        // Initialize head
        HoldNoteHead head = headObj.GetComponent<HoldNoteHead>();
        if (head != null)
        {
            head.Initialize(hitTime, holdLength, laneIndex, tapPoints[laneIndex]);
        }
    }
}