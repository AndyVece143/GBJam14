using UnityEngine;

public class Heart : MonoBehaviour
{
    private Player player;
    public AudioClip pickUpSound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = Player.FindAnyObjectByType<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player.health = 10;
            SoundManager.instance.PlaySound(pickUpSound);
            Destroy(gameObject);
        }
    }
}
