using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public SoloBigDialogue dialogue;
    public DialogueData data;
    public BoxCollider2D boxCollider;
    public Player player;

    void Start()
    {
        player = Player.FindAnyObjectByType<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && player.state != Player.State.NoMove)
        {
            player.StopMoving();
            SoloBigDialogue newDialogue = Instantiate(dialogue);
            newDialogue.data = data;
        }
    }
}
