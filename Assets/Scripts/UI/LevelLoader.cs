using GBTemplate;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader instance { get; private set; }
    public GBDisplayController displayController;

    private void Awake()
    {
        instance = this;
        displayController = GBDisplayController.FindAnyObjectByType<GBDisplayController>();
    }

    void Start()
    {
        OnSceneLoad();
    }

    void OnSceneLoad()
    {
        StartCoroutine(displayController.FadeFromWhite(1f));
    }

    public void LoadNextLevel(string sceneName)
    {
        StartCoroutine(LoadLevel(sceneName));
    }

    IEnumerator LoadLevel(string sceneName)
    {
        StartCoroutine(displayController.FadeToWhite(1f));
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(sceneName);

    }
}
