using UnityEngine;

public class HitSFXManager : MonoBehaviour
{
    public static HitSFXManager Instance;

    [Header("Judgement Sounds")]
    [SerializeField] AudioClip perfectSFX;
    [SerializeField] AudioClip greatSFX;
    [SerializeField] AudioClip goodSFX;
    [SerializeField] AudioClip badSFX;
    [SerializeField] AudioClip missSFX;

    private AudioSource audioSource;

    public void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
    }
    public void PlaySFX(string judgement)
    {
        AudioClip clip = null;

        switch (judgement.ToUpper())
        {
            case "PERFECT": clip = perfectSFX; 
                break;
            case "GREAT": clip = greatSFX;
                break;
            case "GOOD": clip = goodSFX; 
                break;
            case "BAD": clip = badSFX; 
                break;
            case "MISS": clip = missSFX; 
                break;
        }

        if (clip != null)
            audioSource.PlayOneShot(clip);
    }

}
