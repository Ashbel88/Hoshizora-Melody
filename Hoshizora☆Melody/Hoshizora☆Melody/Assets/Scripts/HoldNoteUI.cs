using UnityEngine;

public class HoldNoteUI : MonoBehaviour
{
    public Transform bodyParent;    // Empty transform (anchor at tap point)
    public Transform body;          // Body sprite (pivot bottom)
    public Transform tail;          // Tail sprite (pivot bottom)

    private Transform head;         // Head note reference

    private float startTime;
    private float endTime;

    public float lengthPerSecond = 2.0f;

    private bool tailShown = false;

    
    // INIT
  
    public void Initialize(float hitTime, float holdLength)
    {
        startTime = hitTime;
        endTime = hitTime + holdLength;

        // Reset visuals
        body.localScale = new Vector3(body.localScale.x, 0f, body.localScale.z);
        tail.gameObject.SetActive(false);
        tailShown = false;
    }

   
    // CONNECT UI TO HEAD NOTE
    
    public void AttachToHead(Transform headTransform)
    {
        head = headTransform;
    }

    void Update()
    {
        float song = AudioManager.Instance.GetSongTime();

        // Follow head position & rotation
        if (head != null)
        {
            bodyParent.position = head.position;
            bodyParent.rotation = head.rotation;
        }

        if (song < startTime)
            return;

        float t = Mathf.InverseLerp(startTime, endTime, song);
        float fullLength = lengthPerSecond * (endTime - startTime);
        float currentLength = fullLength * t;

        // Stretch body
        body.localScale = new Vector3(body.localScale.x, currentLength, body.localScale.z);

        // Show tail at end
        if (song >= endTime)
        {
            if (!tailShown)
            {
                tail.gameObject.SetActive(true);
                tailShown = true;
            }

            tail.position = bodyParent.position + bodyParent.up * currentLength;
        }
    }

    public void OnRelease(float releasedTime)
    {
        float diff = Mathf.Abs(releasedTime - endTime);

        if (diff < 0.10f)
            Debug.Log("Hold Release PERFECT");
        else if (diff < 0.20f)
            Debug.Log("Hold Release GREAT");
        else
            Debug.Log("Hold Release BAD");

        Destroy(gameObject);
    }
}
