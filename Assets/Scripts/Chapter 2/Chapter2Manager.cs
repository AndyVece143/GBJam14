using GBTemplate;
using UnityEngine;

public class Chapter2Manager : MonoBehaviour
{
    public GBDisplayController displayController;
    public AudioClip chaseMusic;
    public AudioClip regularMusic;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        displayController.UpdateColorPalette(38);
        Debug.Log(MusicPlayer.instance.source.clip.name);
        Debug.Log(regularMusic.name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeToChaseMusic()
    {
        MusicPlayer.instance.ChangeSong(chaseMusic);
    }

    public void ChangeToRegularMusic()
    {
        if (MusicPlayer.instance.source.clip.name != regularMusic.name)
        {
            MusicPlayer.instance.ChangeSong(regularMusic);
        }
    }
}
