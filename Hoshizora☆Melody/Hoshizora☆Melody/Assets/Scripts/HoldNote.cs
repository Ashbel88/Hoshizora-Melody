using UnityEngine;

public class HoldNote : MonoBehaviour
{
    // WORK IN PROGRESS

    [Header("Hold Visuals")]
    public Transform head;
    public Transform bodyParent;
    public Transform body;
    public Transform tail;

    private SpriteRenderer bodySR;

    [Header("Hold Timing")]
    public float hitTime;
    public float holdDuration;
    private float tailTime;

    private bool initialized = false;

    public void InitializeHoldNote(float hitTime, float holdLength, Vector3 direction)
    {
        this.hitTime = hitTime;
        this.holdDuration = holdLength;
        this.tailTime = hitTime + holdLength;

        travelDirection = direction;

        RotateBodyParent();

        body.localScale = new Vector3(body.localScale.x, 0f, body.localScale.z);
        tail.gameObject.SetActive(false);

        initialized = true;
    }

    private void Awake()
    {
        bodySR = body.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (!initialized) return;

        UpdateBodyGrowth();
        UpdateTailSpawn();
    }

    
    // ROTATION

    public Vector3 travelDirection;

    void RotateBodyParent()
    {
        if (travelDirection == Vector3.zero) return;

        float angle = Mathf.Atan2(travelDirection.y, travelDirection.x) * Mathf.Rad2Deg - 90f;
        bodyParent.rotation = Quaternion.Euler(0, 0, angle);
    }

    // BODY GROWTH
  

    void UpdateBodyGrowth()
    {
        float songTime = AudioManager.Instance.GetSongTime();


        bodyParent.position = head.position;

        float start = hitTime;
        float end = tailTime;

        if (songTime < start)
        {
            body.localScale = new Vector3(body.localScale.x, 0f, body.localScale.z);
            return;
        }

        float t = Mathf.InverseLerp(start, end, songTime);

        float targetLength = 4f;
        body.localScale = new Vector3(
            body.localScale.x,
            targetLength * t,
            body.localScale.z
        );
    }


    // TAIL SPAWN

    void UpdateTailSpawn()
    {
        float songTime = AudioManager.Instance.GetSongTime();

        if (!tail.gameObject.activeSelf && songTime >= tailTime)
        {
            float bodyHeight = bodySR.bounds.size.y;
            tail.position = bodyParent.position - bodyParent.up * bodyHeight;

            tail.gameObject.SetActive(true);
        }
    }
}
