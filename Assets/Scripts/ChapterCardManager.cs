using GBTemplate;
using System.Collections;
using TMPro;
using UnityEngine;

public class ChapterCardManager : MonoBehaviour
{
    public TextMeshProUGUI chapterText;
    public Animator anim;
    public GBDisplayController displayController;
    public float duration;
    public string sceneName;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        switch (StaticData.goingToChapter)
        {
            case 0:
                chapterText.text = "Chapter 1";
                anim.SetInteger("chapter", 0);
                displayController.UpdateColorPalette(18);
                break;

            case 1:
                chapterText.text = "Chapter 2";
                anim.SetInteger("chapter", 1);
                displayController.UpdateColorPalette(22);
                break;

            case 2:
                chapterText.text = "Chapter 3";
                anim.SetInteger("chapter", 2);
                displayController.UpdateColorPalette(24);
                break;
        }

        StartCoroutine(ChapterCardWait());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator ChapterCardWait()
    {
        yield return new WaitForSeconds(duration);

        switch (StaticData.goingToChapter)
        {
            case 0:
                sceneName = "Chapter1";
                break;
            case 1:
                sceneName = "Chapter2";
                break;
            case 2:
                sceneName = "Chapter3";
                break;

        }

        LevelLoader.instance.LoadNextLevel(sceneName);
    }
}
