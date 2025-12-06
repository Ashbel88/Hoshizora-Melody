using UnityEngine;

public class NotePath : MonoBehaviour
{
    [Header("Path Points")]
    public Vector3 startPoint;
    public Vector3 tapPoint; 
    public Vector3 endPoint;

    [Header("Curve Setting")]
    public float controlHeight = 1.0f;

    [Header("Scaling")]
    public AnimationCurve scaleCurve;
    public Vector3 maxScale;
    public float scaleEndTime = 0.8f;

    [Header("Timing")]
    public float travelTime = 1.5f; // time to reach tap point
    public float timer = 0f;

    [Header("Behavior Flags")]
    public bool stopAtTap = false;   // For Hold heads
    public bool autoDelete = true;   // For Normal notes

    void Update()
    {
        NoteMovement();
    }

    private void NoteMovement()
    {
        timer += Time.deltaTime;
        float t = Mathf.Clamp01(timer / travelTime);

        Vector3 pos = Vector3.Lerp(startPoint, tapPoint, t);

       
        // NORMAL NOTE BEHAVIOR
     
        if (!stopAtTap)
        {
            if (timer > travelTime)
            {
                float t2 = Mathf.Clamp01((timer - travelTime) / 0.4f);
                pos = Vector3.Lerp(tapPoint, endPoint, t2);
            }
        }

        // HOLD NOTE HEAD BEHAVIOR (Work in Progress)

        else
        {
            if (timer >= travelTime)
            {
                float t2 = Mathf.Clamp01((timer - travelTime) / 0.4f);
                pos = Vector3.Lerp(tapPoint, endPoint, t2);
            }
        }

        transform.position = pos;

        NoteScale(t);

        if (autoDelete)
            CheckDelete();
    }

    private void NoteScale(float t)
    {
        float scaleT = Mathf.Clamp01(t / scaleEndTime);
        float scaleFactor = (scaleCurve != null) ?
            scaleCurve.Evaluate(scaleT) :
            scaleT;

        transform.localScale = maxScale * scaleFactor;
    }

    private void CheckDelete()
    {
        if (Vector3.Distance(transform.position, endPoint) < 0.15f)
        {
            Destroy(gameObject);
        }
    }
}