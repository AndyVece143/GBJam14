using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class PlayerCollision : MonoBehaviour
{
    public Player player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("DIE");
            if (player.state == Player.State.Standard || player.state == Player.State.Sword)
            {
                if (player.iFrames == false)
                {
                    StartCoroutine(player.Knockback(collision.gameObject.transform.position));
                    StartCoroutine(player.IFrames());
                }
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("DIE");
            if (player.state == Player.State.Standard || player.state == Player.State.Sword)
            {
                if (player.iFrames == false)
                {
                    StartCoroutine(player.Knockback(collision.gameObject.transform.position));
                    StartCoroutine(player.IFrames());
                }
            }
        }
    }
}
