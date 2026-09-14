using TMPro;
using UnityEngine;
using System.Collections;

public class InspectBox : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;
    private int index;
    public Canvas canvas;
    public GameObject textBox;
    private Vector3 textBoxPosition;
    private Vector3 textBoxEndPosition;
    public float duration;
    public AudioClip audioClip;
    private const string HTML_ALPHA = "<color=#00000000>";
    public bool ready = false;
    public float dampSpeed;
    public Player player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
        player = Player.FindAnyObjectByType<Player>();
        textComponent.text = string.Empty;
        Debug.Log(textBox.transform.position);
        SetPosition();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (ready == true)
            {
                NextLine();
            }
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
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            StartCoroutine(MoveSpriteEnd());
        }
    }

    void SetPosition()
    {
        textBoxPosition = textBox.transform.localPosition;
        Debug.Log(textBox.transform.position);
        //textBox.transform.position = new Vector3(textBox.transform.position.x, textBox.transform.position.y - 50f, textBox.transform.position.z);
        textBox.transform.localPosition = new Vector3(textBoxPosition.x, textBoxPosition.y - 50, textBoxPosition.z);
        //textBox.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 100f);
        Debug.Log(textBox.transform.position);
        textBoxEndPosition = textBox.transform.localPosition;
        StartCoroutine(MoveSpriteBeginning());

    }

    IEnumerator TypeLine()
    {
        int i = 2;
        string originalText = lines[index];
        string displayedText = "";
        int alphaIndex = 0;

        foreach (char c in lines[index].ToCharArray())
        {
            alphaIndex++;
            textComponent.text = originalText;
            displayedText = textComponent.text.Insert(alphaIndex, HTML_ALPHA);
            textComponent.text = displayedText;

            i++;
            if (i == 3)
            {
                SoundManager.instance.PlaySound(audioClip);
                i = 0;
            }
            yield return new WaitForSeconds(textSpeed);
        }
        ready = true;
    }

    IEnumerator MoveSpriteBeginning()
    {
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = 1.0f - Mathf.Exp(-dampSpeed * Time.deltaTime);
            textBox.transform.localPosition = Vector3.Lerp(textBox.transform.localPosition, textBoxPosition, t);
            yield return null;
        }
        StartDialogue();
    }

    IEnumerator MoveSpriteEnd()
    {
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            float t = 1.0f - Mathf.Exp(-dampSpeed * Time.deltaTime);
            textBox.transform.localPosition = Vector3.Lerp(textBox.transform.localPosition, textBoxEndPosition, t);
            yield return null;
        }

        //if (interactableObject)
        //{
        //    interactableObject.interactable = true;
        //}

        player.StartMoving();
        //mainCamera.state = mainCamera.initialState;
        Destroy(gameObject);
    }
}
