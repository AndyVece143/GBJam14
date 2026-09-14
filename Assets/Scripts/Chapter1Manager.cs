using GBTemplate;
using System.Collections;
using UnityEngine;

public class Chapter1Manager : MonoBehaviour
{
    public GBDisplayController displayController;
    public Player player;

    public SoloBigDialogue dialogue;
    public DialogueData data1;
    public GameObject entranceWall;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        displayController.UpdateColorPalette(3);
        StartCoroutine(Cutscene());
        player.state = Player.State.ScreenTrans;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator Cutscene()
    {
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(player.GoToPlace(new Vector2(0, 8), 3, new Vector2(1, 0)));
        yield return new WaitForSeconds(3.5f);
        SoloBigDialogue newDialogue = Instantiate(dialogue);
        newDialogue.data = data1;
        entranceWall.SetActive(true);
    }
}
