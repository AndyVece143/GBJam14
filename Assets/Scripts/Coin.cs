using UnityEngine;

public class Coin : MonoBehaviour
{
    private Player player;
    public AudioClip pickUpSound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = Player.FindAnyObjectByType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player.gold++;
            SoundManager.instance.PlaySound(pickUpSound);
            Destroy(gameObject);
        }
    }
}
