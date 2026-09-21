using GBTemplate;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    public GBDisplayController displayController;
    public Button playButton;
    public Button creditsTitleButton;
    public Button chapter1Button;

    public GameObject mainTitle;
    public GameObject credits;
    public GameObject chapters;
    public Camera mainCamera;
    public float buttonTimer;
    public float duration;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playButton.Select();
        displayController.UpdateColorPalette(41);

        StaticData.chapter1CutsceneWatch = false;
        StaticData.chapter2CutsceneWatch = false;
        StaticData.chapter3CutsceneWatch = false;
        StaticData.bossCutsceneWatch = false;
        StaticData.goingToChapter = 0;
        StaticData.goldAmount = 0;
        //EventSystem.current.SetSelectedGameObject(playButton.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        buttonTimer -= Time.deltaTime;
    }

    public void GoToCreditsScreen()
    {
        if (buttonTimer <= 0)
        {
            StartCoroutine(MoveElements(false));
            buttonTimer = duration + 0.1f;
            creditsTitleButton.Select();
        }
    }

    public void CreditsToMain()
    {
        if (buttonTimer <= 0)
        {
            StartCoroutine(MoveElements(true));
            buttonTimer = duration + 0.1f;
            playButton.Select();
        }
    }

    public void GoToChapterScreen()
    {
        if (buttonTimer <= 0)
        {
            StartCoroutine(MoveElements(true));
            buttonTimer = duration + 0.1f;
            chapter1Button.Select();
        }
    }

    public void ChaptersToMain()
    {
        if (buttonTimer <= 0)
        {
            StartCoroutine(MoveElements(false));
            buttonTimer = duration + 0.1f;
            playButton.Select();
        }
    }

    public void StartChapter(int i)
    {
        StaticData.goingToChapter = i;
        LevelLoader.instance.LoadNextLevel("ChapterCard");
    }

    IEnumerator MoveElements(bool right)
    {
        float time = 0;
        float moveAmount = 160;

        if (!right)
        {
            moveAmount = -moveAmount;
        }

        //Vector2 titleVector = new Vector2(mainTitle.transform.position.x + moveAmount / 80, 8);
        //Vector2 creditsVector = new Vector2(credits.transform.position.x + moveAmount / 80, 8);
        //Vector2 chaptersVector = new Vector2(chapters.transform.position.x + moveAmount / 80, 8);
        //Vector3 cameraVector = new Vector3(mainCamera.transform.position.x - moveAmount, 8, -10);

        Vector2 titleVector = new Vector2(mainTitle.transform.position.x, 8);
        Vector2 creditsVector = new Vector2(credits.transform.position.x, 8);
        Vector2 chaptersVector = new Vector2(chapters.transform.position.x, 8);
        Vector3 cameraVector = new Vector3(mainCamera.transform.position.x - moveAmount, 8, -10);

        Vector2 mainOG = mainTitle.transform.position;
        Vector2 creditsOG = credits.transform.position;
        Vector2 chaptersOG = chapters.transform.position;
        Vector3 cameraOG = mainCamera.transform.position;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            mainTitle.transform.position = Vector2.Lerp(mainOG, titleVector, t);
            credits.transform.position = Vector2.Lerp(creditsOG, creditsVector, t);
            chapters.transform.position = Vector2.Lerp(chaptersOG, chaptersVector, t);
            mainCamera.transform.position = Vector3.Lerp(cameraOG, cameraVector, t);
            yield return null;
        }
    }
}
