using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public Button defaultButton;
    public TextMeshProUGUI musicText;
    public TextMeshProUGUI soundText;
    public GameObject pauseMenuUI;
    public Button resumeButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EventSystem.current.SetSelectedGameObject(defaultButton.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject.name == "Sound Label")
        {
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                SoundManager.instance.source.volume += 0.1f;
                SoundManager.instance.source.volume = Mathf.Clamp01(SoundManager.instance.source.volume);
                UpdateText();
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                SoundManager.instance.source.volume -= 0.1f;
                SoundManager.instance.source.volume = Mathf.Clamp01(SoundManager.instance.source.volume);
                UpdateText();
            }
        }

        if (EventSystem.current.currentSelectedGameObject.name == "Music Label")
        {
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                MusicPlayer.instance.source.volume += 0.1f;
                MusicPlayer.instance.source.volume = Mathf.Clamp01(MusicPlayer.instance.source.volume);
                UpdateText();
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                MusicPlayer.instance.source.volume -= 0.1f;
                MusicPlayer.instance.source.volume = Mathf.Clamp01(MusicPlayer.instance.source.volume);
                UpdateText();
            }
        }
    }

    void UpdateText()
    {
        soundText.text = (SoundManager.instance.source.volume * 10).ToString("00");
        musicText.text = (MusicPlayer.instance.source.volume * 10).ToString("00");
    }

    public void BackToPause()
    {
        gameObject.SetActive(false);
        pauseMenuUI.SetActive(true);
        resumeButton.Select();
    }
}
