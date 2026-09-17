using GBTemplate;
using UnityEngine;

public class Chapter2Manager : MonoBehaviour
{
    public GBDisplayController displayController;
    public AudioClip chaseMusic;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        displayController.UpdateColorPalette(38);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeToChaseMusic()
    {
        MusicPlayer.instance.ChangeSong(chaseMusic);
    }
}
