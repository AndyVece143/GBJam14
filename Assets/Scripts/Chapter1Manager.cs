using GBTemplate;
using System.Collections;
using UnityEngine;

public class Chapter1Manager : MonoBehaviour
{
    public GBDisplayController displayController;
    public Player player;

    public SoloBigDialogue dialogue;
    public DialogueData data1;
    public DialogueData data2;
    public BigDialogue bigDialogue;
    public DialogueData data3;
    public DialogueData data4;

    public GameObject entranceWall;
    public GameObject sign;
    public float signDuration;
    public float spinSpeed;
    private int progress = 0;

    public AudioClip paletteChange;
    public AudioClip signSpin;
    public AudioClip signSpinReverse;

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
            case 2:
                StartCoroutine(Cutscene5());
                break;
        }
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

    private IEnumerator Cutscene2()
    {
        StartCoroutine(player.GoToPlace(new Vector2(780, 144), 2, new Vector2(1, 0)));
        yield return new WaitForSeconds(3);
        SoloBigDialogue newDialogue = Instantiate(dialogue);
        newDialogue.data = data2;
    }

    private IEnumerator Cutscene3()
    {
        yield return new WaitForSeconds(1);
        displayController.UpdateColorPalette(12);
        SoundManager.instance.PlaySound(paletteChange);
        yield return new WaitForSeconds(1);
        player.transform.localScale = new Vector3(-1, 1, 1);
        yield return new WaitForSeconds(1);
        player.transform.localScale = new Vector3(1, 1, 1);
        yield return new WaitForSeconds(1);

        float time = 0;
        Vector2 startingPos = sign.transform.position;
        SoundManager.instance.PlaySound(signSpin);
        while (time < signDuration)
        {
            time += Time.deltaTime;
            sign.transform.Rotate(Vector3.forward * spinSpeed *  Time.deltaTime);

            float t = time / signDuration;
            sign.transform.position = Vector2.Lerp(startingPos, new Vector2(810, 144), t);
            yield return null;
        }

        sign.transform.rotation = new Quaternion(0, 0, 0, 0);
        sign.transform.position = new Vector2(810, 144);
        yield return new WaitForSeconds(1);
        BigDialogue newDialogue = Instantiate(bigDialogue);
        newDialogue.data = data3;
    }

    private IEnumerator Cutscene4()
    {
        Debug.Log("HELP");
        yield return new WaitForSeconds(1);

        float time = 0;
        Vector2 startingPos = sign.transform.position;
        SoundManager.instance.PlaySound(signSpinReverse);
        while (time < signDuration)
        {
            time += Time.deltaTime;
            sign.transform.Rotate(Vector3.forward * -spinSpeed * Time.deltaTime);

            float t = time / signDuration;
            sign.transform.position = Vector2.Lerp(startingPos, new Vector2(810, 220), t);
            yield return null;
        }

        displayController.UpdateColorPalette(3);
        SoundManager.instance.PlaySound(paletteChange);
        yield return new WaitForSeconds(1);
        SoloBigDialogue newDialogue = Instantiate(dialogue);
        newDialogue.data = data4;
    }
    private IEnumerator Cutscene5()
    {
        StartCoroutine(player.GoToPlace(new Vector2(900, 144), 4, new Vector2(1, 0)));
        yield return new WaitForSeconds(4);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StartCoroutine(Cutscene2());
        }
    }
}
