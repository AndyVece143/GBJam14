using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; }
    public AudioSource source;

    private void Awake()
    {
        instance = this;
        source = GetComponent<AudioSource>();
        source.volume = StaticData.soundVolume;

    }

    public void PlaySound(AudioClip _sound)
    {
        source.PlayOneShot(_sound);
    }
}