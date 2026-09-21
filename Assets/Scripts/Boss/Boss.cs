using System.Collections;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public float speed;
    public BoxCollider2D boxCollider;
    public Rigidbody2D body;

    public enum State
    {
        NoMove,
        Standard,
    }

    public State state;
    public int health;
    public bool iFrames;
    public float IFrameTimer;
    public AudioClip damageSound;
    public Explosion explosion;
    public Fireball fireball;
    public float fireTimer;
    private float fireTimerMax;
    public Player player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fireTimerMax = fireTimer;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Standard:
                Movement();
                break;
            case State.NoMove:
                break;
        }
    }

    private void Movement()
    {
        body.linearVelocity = new Vector2(speed, body.linearVelocity.y);

        fireTimer -= Time.deltaTime;
        if (fireTimer <= 0)
        {
            SpawnFireball();
            fireTimer = fireTimerMax;
        }
    }

    private void SpawnFireball()
    {
        Fireball newFire = Instantiate(fireball, transform);
        int i = Random.Range(0, 2);

        switch (i)
        {
            case 0:
                newFire.transform.position = new Vector2(-100, player.transform.position.y);
                break;
            case 1:
                newFire.transform.position = new Vector2(100, player.transform.position.y);
                newFire.transform.localScale = new Vector3(-1, 1, 1);
                newFire.speed = -newFire.speed;
                break;
        }


    }

    public void BeginFight()
    {
        state = State.Standard;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            speed = -speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Sword")
        {
            if (iFrames == false)
            {
                StartCoroutine(TakeDamage());
            }
        }
    }

    private IEnumerator TakeDamage()
    {
        health -= 1;

        if (health > 0)
        {
            SoundManager.instance.PlaySound(damageSound);
            iFrames = true;

            float time = 0;
            while (time < IFrameTimer)
            {
                time += Time.deltaTime;
                if (gameObject.GetComponent<SpriteRenderer>().color == Color.white)
                {
                    gameObject.GetComponent<SpriteRenderer>().color = Color.black;
                }
                else if (gameObject.GetComponent<SpriteRenderer>().color == Color.black)
                {
                    gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                }

                yield return null;
            }

            iFrames = false;
            gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }

        else
        {
            Explosion newExplosion = Instantiate(explosion);
            newExplosion.transform.position = transform.position;
            Destroy(gameObject);
        }
    }
}
