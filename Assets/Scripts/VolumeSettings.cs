using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider musicSlider;

    //private void Start()
    //{
    //    SetMusicVolume();
    //}

    public void SetMusicVolume()
    {
        float volume = Mathf.Clamp(musicSlider.value, 0.0001f, 1f); // Evita valores inválidos
        myMixer.SetFloat("music", Mathf.Log10(volume) * 20);
    }
}
