using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class Slime : MonoBehaviour
{
    public float speed;
    public Rigidbody2D body;
    public BoxCollider2D boxCollider;
    public Animator anim;
    public AreaScreen areaScreen;
    public Vector2 movement;

    public float moveTimer;
    private float moveTimerMax;

    public bool iFrames;
    public float IFrameTimer;
    private bool inKnockback = false;
    public float thrust;
    public float knockbackDuration;

    public float health;
    public Explosion explosion;
    public AudioClip damageSound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        moveTimerMax = moveTimer;
    }

    // Update is called once per frame
    void Update()
    {
        if (inKnockback == false)
        {
            body.linearVelocity = movement * speed;
            moveTimer -= Time.deltaTime;

            if (moveTimer <= 0)
            {
                movement = ChangeDirection();
                moveTimer = moveTimerMax;
            }
        }
    }

    private Vector2 ChangeDirection()
    {
        int i = Random.Range(0, 4);

        switch (i)
        {
            case 0:
                return new Vector2(1, 0);
            case 1:
                return new Vector2(-1, 0);
            case 2:
                return new Vector2(0, 1);
            case 3:
                return new Vector2(0, -1);
        }

        return new Vector2(0, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Sword")
        {
            if (iFrames == false)
            {
                StartCoroutine(Knockback(collision.gameObject.transform.position));
                StartCoroutine(IFrames());
            }
        }
    }

    private IEnumerator IFrames()
    {
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

    private IEnumerator Knockback(Vector2 damagePosition)
    {
        health -= 1;

        if (health > 0)
        {
            SoundManager.instance.PlaySound(damageSound);
            inKnockback = true;
            Vector2 direction = (Vector2)transform.position - damagePosition;
            direction = direction.normalized;

            body.AddForce(direction * thrust, ForceMode2D.Impulse);

            yield return new WaitForSeconds(knockbackDuration);

            body.linearVelocity = Vector2.zero;
            inKnockback = false;
        }

        else
        {
            Explosion newExplosion = Instantiate(explosion);
            newExplosion.transform.position = transform.position;
            Destroy(gameObject);
        }
    }
}
