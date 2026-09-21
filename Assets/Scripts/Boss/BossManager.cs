using GBTemplate;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossManager : MonoBehaviour
{
    public GBDisplayController displayController;
    public Boss boss;
    public List<LockedDoor> doorList;
    public bool bossDefeated;
    public Player player;
    public SoloBigDialogue dialogue;
    public DialogueData data1;
    public DialogueData data2;
    public DialogueData data3;
    public int progress = 0;
    public AudioClip doorSound;
    public AudioClip bossMusic;
    public Image cardImage;
    public GameObject smallCard;
    public AudioClip explosion;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        displayController.UpdateColorPalette(44);

        switch (StaticData.bossCutsceneWatch)
        {
            case false:
                StaticData.bossCutsceneWatch = true;
                StartCoroutine(Cutscene());
                player.state = Player.State.ScreenTrans;
                break;
            case true:
                BeginTheFight();
                progress++;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (boss.health <= 0 && bossDefeated == false)
        {
            for (int i = 0; i < doorList.Count; i++)
            {
                doorList[i].OpenTheDoor();
            }
            bossDefeated = true;
            SoundManager.instance.PlaySound(doorSound);
            MusicPlayer.instance.StopMusic();
        }
    }

    public void StartCutscene()
    {
        switch (progress)
        {
            case 0:
                BeginTheFight();
                progress++;
                break;

            case 1:
                StartCoroutine(Cutscene3());
                progress++;
                break;

            case 2:
                StartCoroutine(Cutscene4());
                break;
        }
    }

    IEnumerator Cutscene()
    {
        yield return new WaitForSeconds(2.5f);
        SoloBigDialogue newDialogue = Instantiate(dialogue);
        newDialogue.data = data1;
    }

    void BeginTheFight()
    {
        boss.BeginFight();
        MusicPlayer.instance.ChangeSong(bossMusic);
    }

    IEnumerator Cutscene2()
    {
        StartCoroutine(player.GoToPlace(new Vector2(0, 110), 2, new Vector2(0, 1)));
        yield return new WaitForSeconds(3);
        SoloBigDialogue newDialogue = Instantiate(dialogue);
        newDialogue.data = data2;
    }

    private IEnumerator Cutscene3()
    {
        StartCoroutine(player.GoToPlace(new Vector2(0, 127), 2, new Vector2(0, 1)));
        yield return new WaitForSeconds(3);
        StartCoroutine(displayController.FadeToWhite(1));
        yield return new WaitForSeconds(1.1f);
        cardImage.GetComponent<Image>().enabled = true;
        smallCard.SetActive(false);

        displayController.UpdateColorPalette(43);
        StartCoroutine(displayController.FadeFromWhite(1));
        yield return new WaitForSeconds(1);
        SoundManager.instance.PlaySound(explosion);
        yield return new WaitForSeconds(3);
        StartCoroutine(displayController.FadeToWhite(1));
        yield return new WaitForSeconds(1.1f);
        cardImage.GetComponent<Image>().enabled = false;

        displayController.UpdateColorPalette(44);
        StartCoroutine(displayController.FadeFromWhite(1));
        yield return new WaitForSeconds(2);

        SoloBigDialogue newDialogue = Instantiate(dialogue);
        newDialogue.data = data3;
    }

    private IEnumerator Cutscene4()
    {
        StartCoroutine(player.GoToPlace(new Vector2(0, 90), 4, new Vector2(0, -1)));
        yield return new WaitForSeconds(2.5f);
        StaticData.goingToChapter = 3;
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
