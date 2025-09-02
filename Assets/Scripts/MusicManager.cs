using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    [Header("Assign etmezsen otomatik ekler")]
    public AudioSource audioSource;
    public AudioClip musicClip;
    public bool playOnStart = true;
    public bool loop = true;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.loop = loop;
        if (musicClip != null && audioSource.clip == null)
            audioSource.clip = musicClip;
    }

    void Start()
    {
        if (playOnStart && audioSource.clip != null && !audioSource.isPlaying)
            audioSource.Play();
    }
}
