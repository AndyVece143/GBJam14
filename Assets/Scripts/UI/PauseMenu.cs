using GBTemplate;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public GameObject pauseMenuUI;
    public Player player;
    public GameObject optionsMenuUI;
    public Button musicButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && player.state == Player.State.Standard)
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        //Debug.Log(EventSystem.current.currentSelectedGameObject);
    }

    public void Resume()
    {
        Debug.Log("Resume");
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void Options()
    {
        pauseMenuUI.SetActive(false);
        optionsMenuUI.SetActive(true);
        musicButton.Select();
    }
}
