using GBTemplate;
using System.Collections;
using UnityEngine;

public class Chapter3Manager : MonoBehaviour
{
    public GBDisplayController displayController;
    public Player player;
    public SoloBigDialogue dialogue;
    public DialogueData data1;
    public DialogueData data2;
    public GameObject entranceWall;
    private int progress = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        displayController.UpdateColorPalette(41);

        switch (StaticData.chapter3CutsceneWatch)
        {
            case false:
                StaticData.chapter3CutsceneWatch = true;
                StartCoroutine(Cutscene());
                player.state = Player.State.ScreenTrans;
                break;
            case true:
                player.transform.position = new Vector2(0, -20);
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
                break;
        }
    }

    IEnumerator Cutscene()
    {
        yield return new WaitForSeconds(1);
        StartCoroutine(player.GoToPlace(new Vector2(0, -20), 3, new Vector2(0, 1)));
        yield return new WaitForSeconds(3.5f);
        SoloBigDialogue newDialogue = Instantiate(dialogue);
        newDialogue.data = data1;
        entranceWall.SetActive(true);
    }

    private IEnumerator Cutscene2()
    {
        StartCoroutine(player.GoToPlace(new Vector2(-480, 656), 3, new Vector2(0, 1)));
        yield return new WaitForSeconds(3.5f);
        SoloBigDialogue newDialogue = Instantiate(dialogue);
        newDialogue.data = data2;
    }

    private IEnumerator Cutscene3()
    {
        StartCoroutine(player.GoToPlace(new Vector2(-480, 715), 4, new Vector2(0, 1)));
        yield return new WaitForSeconds(2f);
        LevelLoader.instance.LoadNextLevel("Boss");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StartCoroutine(Cutscene2());
        }
    }
}
