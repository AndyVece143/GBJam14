using GBTemplate;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
    public static DeathScreen instance { get; private set; }
    public Image graveImage;
    public Button tryAgainButton;
    public Button giveUpButton;
    public GBDisplayController displayController;
    public AudioClip deadMusic;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
        displayController = GBDisplayController.FindAnyObjectByType<GBDisplayController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator OnDeath()
    {
        MusicPlayer.instance.StopMusic();
        yield return new WaitForSeconds(2);
        StartCoroutine(displayController.FadeToBlack(1f));
        yield return new WaitForSeconds(1.1f);
        graveImage.GetComponent<Image>().enabled = true;
        displayController.UpdateColorPalette(40);
        MusicPlayer.instance.ChangeSong(deadMusic);
        StartCoroutine(displayController.FadeFromBlack(1f));
        yield return new WaitForSeconds(3);

        //tryAgainButton.enabled = true;
        tryAgainButton.gameObject.SetActive(true);
        //giveUpButton.enabled = true;
        giveUpButton.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(tryAgainButton.gameObject);
    }

    public void TryAgain()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        LevelLoader.instance.LoadNextLevel(currentScene);
    }
}
