using GBTemplate;
using System.Collections;
using UnityEngine;

public class Chapter3Manager : MonoBehaviour
{
    public GBDisplayController displayController;
    public Player player;
    public SoloBigDialogue dialogue;
    public DialogueData data1;
    public GameObject entranceWall;
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

    IEnumerator Cutscene()
    {
        yield return new WaitForSeconds(1);
        StartCoroutine(player.GoToPlace(new Vector2(0, -20), 3, new Vector2(0, 1)));
        yield return new WaitForSeconds(3.5f);
        SoloBigDialogue newDialogue = Instantiate(dialogue);
        newDialogue.data = data1;
        entranceWall.SetActive(true);
    }
}
