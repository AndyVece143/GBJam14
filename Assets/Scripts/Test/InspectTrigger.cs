using UnityEngine;

public class InspectTrigger : MonoBehaviour
{
    public InspectBox inspectBox;
    public string[] lines;
    public BoxCollider2D boxCollider;
    public Player player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = Player.FindAnyObjectByType<Player>();
        InspectBox newInspectBox = Instantiate(inspectBox);
        newInspectBox.lines = lines;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && player.state != Player.State.NoMove)
        {
            player.StopMoving();
            InspectBox newInspectBox = Instantiate(inspectBox);
            newInspectBox.lines = lines;
        }
    }
}
