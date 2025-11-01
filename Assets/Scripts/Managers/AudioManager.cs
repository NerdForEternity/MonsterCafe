using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [Header("--- Audio Source ---")]
    [SerializeField] AudioMixer Mixer;
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("--- Audio Clip ---")]
    public AudioClip background;
    public AudioClip cookingComplete;
    public AudioClip openingJournal;
    public AudioClip buttonClick;

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
        if (PlayerPrefs.GetInt("MasterMute") == 1)
            Mixer.SetFloat("Volume", -80f);
        else
            Mixer.SetFloat("Volume", Mathf.Log10(PlayerPrefs.GetFloat("Volume")) * 20);
        if (PlayerPrefs.GetInt("MusicMute") == 1)
            musicSource.volume = 0f;
        else
            musicSource.volume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        if (PlayerPrefs.GetInt("SFXMute") == 1)
            SFXSource.volume = 0;
        else
            SFXSource.volume = PlayerPrefs.GetFloat("SFXVolume", 0.75f);
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
