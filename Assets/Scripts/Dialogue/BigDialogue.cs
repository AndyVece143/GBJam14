using TMPro;
using UnityEngine;
using System.Collections;

public class BigDialogue : MonoBehaviour
{
    public TextMeshProUGUI mainText;
    public TextMeshProUGUI nameText;

    public DialogueData data;
    //public string[] lines;
    public float textSpeed;
    private int index;
    //public bool[] talkChanges;
    //public int[] emotionChanges;
    //public string[] names;
    public bool character1Talking;

    public Canvas canvas;

    public Portrait character1;
    public Portrait character2;

    public Color baseColor;
    public Color darkColor;
    public float duration;
    public float moveDuration;
    private Vector3 character1Position;
    private Vector3 character2Position;

    public GameObject textBox;
    private Vector3 textBoxPosition;

    private Vector3 character1EndPosition;
    private Vector3 character2EndPosition;
    private Vector3 textBoxEndPosition;

    public AudioClip audioClip;
    private bool ending = false;
    //public CameraController mainCamera;

    private const string HTML_ALPHA = "<color=#00000000>";
    public bool ready = false;
    public float dampSpeed;
    //public Player player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainText.text = string.Empty;
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
        mainText.text = string.Empty;
        //mainCamera = CameraController.FindAnyObjectByType<CameraController>();
        //nameText.text = names[0];

        nameText.text = data.Sentences[0].name;
        //player = Player.FindAnyObjectByType<Player>();

        BeginningSprite();
        SetPositions();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            if (ready == true)
            {
                NextLine();
            }
        }

        if (Input.GetKeyDown(KeyCode.L) && ending == false)
        {
            StopAllCoroutines();
            StartCoroutine(MoveSpritesEnd());
        }
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    void NextLine()
    {
        ready = false;
        //if (index < lines.Length - 1)
        //{
        //    index++;

        //    if (talkChanges[index] == true)
        //    {
        //        ChangeBothSprites();
        //    }
        //    ChangeEmotions();
        //    mainText.text = string.Empty;
        //    nameText.text = names[index];
        //    StartCoroutine(TypeLine());
        //}
        if (index < data.Sentences.Count - 1)
        {
            index++;

            if (index > 0 && data.Sentences[index].name != data.Sentences[index - 1].name)
            {
                ChangeBothSprites();
            }
            ChangeEmotions();
            mainText.text = string.Empty;
            nameText.text = data.Sentences[index].name;
            StartCoroutine(TypeLine());
        }
        else
        {
            StartCoroutine(MoveSpritesEnd());
        }
    }

    void BeginningSprite()
    {
        if (!character1.isActiveSpeaker)
        {
            StartCoroutine(ChangeSprite(false, character1));
        }
        if (!character2.isActiveSpeaker)
        {
            StartCoroutine(ChangeSprite(false, character2));
        }
        ChangeEmotions();
    }

    void ChangeBothSprites()
    {
        if (character1.isActiveSpeaker)
        {
            StartCoroutine(ChangeSprite(false, character1));
            StartCoroutine(ChangeSprite(true, character2));
        }

        else
        {
            StartCoroutine(ChangeSprite(true, character1));
            StartCoroutine(ChangeSprite(false, character2));
        }
    }

    void ChangeEmotions()
    {
        if (character1.isActiveSpeaker)
        {
            character1.ChangeEmotion(data.Sentences[index].emotion);
        }

        if (character2.isActiveSpeaker)
        {
            character2.ChangeEmotion(data.Sentences[index].emotion);
        }
    }

    void SetPositions()
    {
        character1Position = character1.transform.position;
        character2Position = character2.transform.position;
        textBoxPosition = textBox.transform.position;

        character1.transform.position = new Vector3(character1Position.x - 7f, character1Position.y, character1Position.z);
        character2.transform.position = new Vector3(character2Position.x + 7f, character2Position.y, character2Position.z);
        textBox.transform.position = new Vector3(textBoxPosition.x, textBoxPosition.y - 6f, textBoxPosition.z);

        character1EndPosition = character1.transform.position;
        character2EndPosition = character2.transform.position;
        textBoxEndPosition = textBox.transform.position;

        StartCoroutine(MoveSpritesBeginning());
    }

    IEnumerator TypeLine()
    {
        int i = 4;
        //string originalText = lines[index];
        string originalText = data.Sentences[index].Text;
        string displayedText = "";
        int alphaIndex = 0;

        foreach (char c in data.Sentences[index].Text.ToCharArray())
        {
            alphaIndex++;
            mainText.text = originalText;
            displayedText = mainText.text.Insert(alphaIndex, HTML_ALPHA);
            mainText.text = displayedText;
            //alphaIndex++;
            i++;
            if (i == 5)
            {
                //SoundManager.instance.PlaySound(audioClip);
                i = 0;
            }
            //Debug.Log(c);

            yield return new WaitForSeconds(textSpeed);
        }
        ready = true;
    }

    IEnumerator ChangeSprite(bool activeSpeaker, Portrait character)
    {
        float time = 0;

        //Become brighter and bigger
        if (activeSpeaker)
        {
            character.isActiveSpeaker = true;
            Vector3 targetSize = character.gameObject.transform.localScale + new Vector3(0.1f, 0.1f, 0.1f);

            if (character.transform.localScale.x < 0)
            {
                targetSize = character.gameObject.transform.localScale + new Vector3(-0.1f, 0.1f, 0.1f);
            }

            //Vector3 targetSize = character.gameObject.transform.localScale + new Vector3(0.1f, 0.1f, 0.1f);
            while (time < duration)
            {
                time += Time.deltaTime;
                character.image.color = Color.Lerp(darkColor, baseColor, time / duration);
                character.gameObject.transform.localScale = Vector3.Lerp(character.gameObject.transform.localScale, targetSize, time / duration);
                yield return null;
            }

        }

        else
        {
            character.isActiveSpeaker = false;
            Vector3 targetSize = character.gameObject.transform.localScale - new Vector3(0.1f, 0.1f, 0.1f);
            if (character.transform.localScale.x < 0)
            {
                targetSize = character.gameObject.transform.localScale - new Vector3(-0.1f, 0.1f, 0.1f);
            }

            while (time < duration)
            {
                time += Time.deltaTime;
                character.image.color = Color.Lerp(baseColor, darkColor, time / duration);
                character.gameObject.transform.localScale = Vector3.Lerp(character.gameObject.transform.localScale, targetSize, time / duration);
                yield return null;
            }

        }
    }

    IEnumerator MoveSpritesBeginning()
    {
        float time = 0;
        while (time < moveDuration)
        {
            time += Time.deltaTime;

            //float t = Mathf.Clamp01(time / moveDuration);
            float t = 1.0f - Mathf.Exp(-dampSpeed * Time.deltaTime);
            //character1.gameObject.transform.position = Vector3.Lerp(character1.gameObject.transform.position, character1Position, time / moveDuration);
            //character2.gameObject.transform.position = Vector3.Lerp(character2.gameObject.transform.position, character2Position, time / moveDuration);
            //textBox.transform.position = Vector3.Lerp(textBox.transform.position, textBoxPosition, time / moveDuration);

            character1.gameObject.transform.position = Vector3.Lerp(character1.gameObject.transform.position, character1Position, t);
            character2.gameObject.transform.position = Vector3.Lerp(character2.gameObject.transform.position, character2Position, t);
            textBox.transform.position = Vector3.Lerp(textBox.transform.position, textBoxPosition, t);
            yield return null;
        }
        StartDialogue();
    }

    IEnumerator MoveSpritesEnd()
    {
        ending = true;
        float time = 0;
        while (time < moveDuration)
        {
            time += Time.deltaTime;
            float t = 1.0f - Mathf.Exp(-dampSpeed * Time.deltaTime);
            //character1.gameObject.transform.position = Vector3.Lerp(character1.gameObject.transform.position, character1EndPosition, time / moveDuration);
            //character2.gameObject.transform.position = Vector3.Lerp(character2.gameObject.transform.position, character2EndPosition, time / moveDuration);
            //textBox.transform.position = Vector3.Lerp(textBox.transform.position, textBoxEndPosition, time / moveDuration);
            character1.gameObject.transform.position = Vector3.Lerp(character1.gameObject.transform.position, character1EndPosition, t);
            character2.gameObject.transform.position = Vector3.Lerp(character2.gameObject.transform.position, character2EndPosition, t);
            textBox.transform.position = Vector3.Lerp(textBox.transform.position, textBoxEndPosition, t);
            yield return null;
        }
        //if (sceneTransition)
        //{
        //    loader.LoadNextLevel(sceneName);
        //}
        //if (letPlayerWalk)
        //{
        //    player.state = Player.State.Standard;
        //    mainCamera.state = CameraController.State.FollowPlayer;
        //}

        //if (canPlayerMove)
        //{
        //    player.StartMoving();
        //    mainCamera.state = CameraController.State.FollowPlayer;
        //}


        Destroy(gameObject);
    }
}