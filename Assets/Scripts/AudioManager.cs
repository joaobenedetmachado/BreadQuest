
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("--------- AUDIO SOURCE ----------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("--------- AUDIO CLIP ----------")]
    public AudioClip background;
    public AudioClip death;
    public AudioClip jump;
    public AudioClip walk;

    [Range(0, 1)] public float volume = 0.1f;

    private void Start()
    {
        musicSource.clip = background;
        musicSource.volume = volume;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
