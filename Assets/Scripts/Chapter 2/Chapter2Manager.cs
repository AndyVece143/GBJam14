using GBTemplate;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Chapter2Manager : MonoBehaviour
{
    public GBDisplayController displayController;
    public AudioClip chaseMusic;
    public AudioClip regularMusic;
    public Player player;
    public SoloBigDialogue dialogue;
    public DialogueData data1;
    public DialogueData data2;
    public DialogueData data3;

    public Image swordImage;

    public GameObject entranceWall;
    public GameObject groundSword;
    public AudioClip explosion;

    private int progress = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        displayController.UpdateColorPalette(38);
        Debug.Log(MusicPlayer.instance.source.clip.name);
        Debug.Log(regularMusic.name);

        switch (StaticData.chapter2CutsceneWatch)
        {
            case false:
                StaticData.chapter2CutsceneWatch = true;
                StartCoroutine(Cutscene());
                player.state = Player.State.ScreenTrans;
                break;

            case true:
                player.transform.position = new Vector2(0, 8);
                entranceWall.SetActive(true);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartCutscene()
    {
        switch (progress)
        {
            case 0:
                StartCoroutine(Cutscene3());
                progress++;
                break;
            case 1:
                StartCoroutine(Cutscene4());
                progress++;
                break;
                //case 2:
                //    StartCoroutine(Cutscene5());
                //    break;
        }
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

    IEnumerator Cutscene()
    {
        yield return new WaitForSeconds(1);
        StartCoroutine(player.GoToPlace(new Vector2(0, 8), 3, new Vector2(1, 0)));
        yield return new WaitForSeconds(3.5f);
        SoloBigDialogue newDialogue = Instantiate(dialogue);
        newDialogue.data = data1;
        entranceWall.SetActive(true);
    }

    private IEnumerator Cutscene2()
    {
        StartCoroutine(player.GoToPlace(new Vector2(800, -400), 3, new Vector2(1, 0)));
        yield return new WaitForSeconds(3);

        StartCoroutine(player.GoToPlace(new Vector2(800, -399.5f), 0.1f, new Vector2(0, 1)));
        yield return new WaitForSeconds(1);
        SoloBigDialogue newDialogue = Instantiate(dialogue);
        newDialogue.data = data2;

    }

    private IEnumerator Cutscene3()
    {
        StartCoroutine(player.GoToPlace(new Vector2(800, -388), 2, new Vector2(0, 1)));
        yield return new WaitForSeconds(3);
        StartCoroutine(displayController.FadeToWhite(1));
        yield return new WaitForSeconds(1.1f);
        swordImage.GetComponent<Image>().enabled = true;
        groundSword.SetActive(false);

        displayController.UpdateColorPalette(28);
        StartCoroutine(displayController.FadeFromWhite(1));
        yield return new WaitForSeconds(1);
        SoundManager.instance.PlaySound(explosion);
        yield return new WaitForSeconds(3);
        StartCoroutine(displayController.FadeToWhite(1));
        yield return new WaitForSeconds(1.1f);
        swordImage.GetComponent<Image>().enabled = false;

        displayController.UpdateColorPalette(38);
        StartCoroutine(displayController.FadeFromWhite(1));
        yield return new WaitForSeconds(2);

        SoloBigDialogue newDialogue = Instantiate(dialogue);
        newDialogue.data = data3;
    }

    private IEnumerator Cutscene4()
    {
        StartCoroutine(player.GoToPlace(new Vector2(800, -363), 3, new Vector2(0, 1)));
        yield return new WaitForSeconds(1);
        StaticData.goingToChapter = 2;
        LevelLoader.instance.LoadNextLevel("ChapterCard");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StartCoroutine(Cutscene2());
        }
    }
}
