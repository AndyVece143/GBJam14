using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer instance { get; private set; }
    public AudioSource source;
    public AudioClip song;

    private void Awake()
    {
        instance = this;
        source = GetComponent<AudioSource>();
        source.volume = StaticData.musicVolume;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayMusic()
    {
        source.Stop();
        source.Play();
    }

    public void StopMusic()
    {
        source.Stop();
    }

    public void ChangeSong(AudioClip newSong)
    {
        source.clip = newSong;
        PlayMusic();
    }
}
