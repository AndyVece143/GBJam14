using UnityEngine;

public class DeveloperTalk : MonoBehaviour
{
    public Player player;
    public BigDialogue dialogue;
    public DialogueData data1;
    public DialogueData data2;

    public BoxCollider2D boxCollider;
    public bool checker = false;
    private int progress = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = Player.FindAnyObjectByType<Player>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (boxCollider.IsTouching(player.boxCollider))
        {
            if (Input.GetKeyDown(KeyCode.X) && player.state != Player.State.NoMove)
            {
                player.inspectIcon.enabled = false;
                player.StopMoving();
                BigDialogue newDialogue = Instantiate(dialogue);

                if (progress == 0)
                {
                    newDialogue.data = data1;

                    progress++;
                }

                else if (progress == 1)
                {
                    newDialogue.data = data2;
                    checker = true;
                }
            }
        }
    }

    public void GenerateDialogue()
    {
        BigDialogue newDialogue = Instantiate(dialogue);

        if (progress == 0)
        {
            newDialogue.data = data1;

            progress++;
        }

        else if (progress == 1)
        {
            newDialogue.data = data2;
            checker = true;
        }
    }
}
