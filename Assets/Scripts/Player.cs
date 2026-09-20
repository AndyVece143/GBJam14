using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float speed;
    public Rigidbody2D body;
    public BoxCollider2D boxCollider;
    public Animator anim;
    public float screenDuration;
    public float screenMoveAmount;
    private Vector2 movement;
    public enum State
    {
        Standard,
        NoMove,
        ScreenTrans,
        Sword,
        Dead,
    }
    public State state;
    public SpriteRenderer inspectIcon;
    public InteractableObject closestObject;
    public DeveloperTalk closestDeveloper;
    public Color dimmedColor;

    public string direction;

    public float swordTime;
    private float swordTimeMax;
    public bool sword;

    public int health = 10;
    public int gold;

    public float thrust;
    public float knockbackDuration;

    private bool inKnockback = false;

    public float IFrameTimer;
    public bool iFrames;

    public GameObject swordBox;
    public Explosion explosion;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();

        inspectIcon.enabled = false;
        swordTimeMax = swordTime;
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
                StayStill();
                break;
            case State.ScreenTrans:
                break;
            case State.Sword:
                SwordMovement();
                break;
            case State.Dead:
                StayStill();
                break;
        }
    }

    private void Movement()
    {
        //float horizontalInput = Input.GetAxis("Horizontal");
        //float verticalInput = Input.GetAxis("Vertical");

        //body.linearVelocity = new Vector2(horizontalInput * speed, verticalInput * speed);

        if (inKnockback == false)
        {
            swordTime = swordTimeMax;
            movement.Set(InputManager.Movement.x, InputManager.Movement.y);
            body.linearVelocity = movement * speed;

            anim.SetFloat("horizontal", movement.x);
            anim.SetFloat("vertical", movement.y);

            if (movement != Vector2.zero)
            {
                anim.SetFloat("lasthorizontal", movement.x);
                anim.SetFloat("lastvertical", movement.y);
            }

            Direction();
        }

    }

    private void Direction()
    {
        //Left
        if (anim.GetFloat("lasthorizontal") == -1 && anim.GetFloat("lastvertical") == 0)
        {
            //Debug.Log("Left");
            direction = "left";
            swordBox.transform.position = new Vector2(transform.position.x - 16, transform.position.y);
        }

        //Right
        if (anim.GetFloat("lasthorizontal") == 1 && anim.GetFloat("lastvertical") == 0)
        {
            //Debug.Log("Right");
            direction = "right";
            swordBox.transform.position = new Vector2(transform.position.x + 16, transform.position.y);
        }

        //Up
        if (anim.GetFloat("lasthorizontal") == 0 && anim.GetFloat("lastvertical") == 1)
        {
            //Debug.Log("Up");
            direction = "up";
            swordBox.transform.position = new Vector2(transform.position.x, transform.position.y + 16);
        }

        //Down
        if (anim.GetFloat("lasthorizontal") == 0 && anim.GetFloat("lastvertical") == -1)
        {
            //Debug.Log("Down");
            direction = "down";
            swordBox.transform.position = new Vector2(transform.position.x, transform.position.y - 16);
        }

        //UpRight
        if (anim.GetFloat("lasthorizontal") > 0 && anim.GetFloat("lasthorizontal") == anim.GetFloat("lastvertical"))
        {
            //Debug.Log("Up Right");
            direction = "up";
            swordBox.transform.position = new Vector2(transform.position.x, transform.position.y + 16);
        }

        //DownLeft
        if (anim.GetFloat("lasthorizontal") < 0 && anim.GetFloat("lasthorizontal") == anim.GetFloat("lastvertical"))
        {
            //Debug.Log("Down Left");
            direction = "down";
            swordBox.transform.position = new Vector2(transform.position.x, transform.position.y - 16);
        }

        //Down Right
        if (anim.GetFloat("lasthorizontal") > 0 && anim.GetFloat("lasthorizontal") + anim.GetFloat("lastvertical") == 0)
        {
            //Debug.Log("Down Right");
            direction = "down";
            swordBox.transform.position = new Vector2(transform.position.x, transform.position.y - 16);
        }

        //UpLeft
        if (anim.GetFloat("lasthorizontal") < 0 && anim.GetFloat("lasthorizontal") + anim.GetFloat("lastvertical") == 0)
        {
            //Debug.Log("Up Left");
            direction = "up";
            swordBox.transform.position = new Vector2(transform.position.x, transform.position.y + 16);
        }
    }

    private void StayStill()
    {
        movement.Set(0, 0);
        body.linearVelocity = Vector2.zero;
        anim.SetFloat("horizontal", movement.x);
        anim.SetFloat("vertical", movement.y);
        if (movement != Vector2.zero)
        {
            anim.SetFloat("lasthorizontal", movement.x);
            anim.SetFloat("lastvertical", movement.y);
        }
    }

    public void Talk(InputAction.CallbackContext context)
    {
        if (context.started && closestObject != null && state == State.Standard)
        {
            closestObject.GenerateTextBox();
            StopMoving();
            inspectIcon.enabled = false;
        }

        if (context.started && closestDeveloper != null && state == State.Standard)
        {
            closestDeveloper.GenerateDialogue();
            StopMoving();
            inspectIcon.enabled = false;
        }

    }

    public void Sword(InputAction.CallbackContext context)
    {
        if (context.started && state == State.Standard && sword == true)
        {
            movement.Set(0, 0);
            body.linearVelocity = Vector2.zero;
            anim.SetTrigger("sword");
            state = State.Sword;
            swordBox.SetActive(true);
        }
    }

    private void SwordMovement()
    {
        swordTime -= Time.deltaTime;

        if (swordTime <= 0)
        {
            Debug.Log("Stop sword");
            state = State.Standard;
            anim.Play("Idle");
            swordBox.SetActive(false);
        }
    }

    public void StopMoving()
    {
        movement.Set(0,0);
        body.linearVelocity = Vector2.zero;

        state = State.NoMove;
    }

    public void StartMoving()
    {
        state = State.Standard;
        anim.Play("Idle");
    }

    public IEnumerator GoToPlace(Vector2 location, float duration, Vector2 direction)
    {
        StartMoving();
        state = State.NoMove;
        float time = 0;
        Vector2 startingPos = transform.position;

        anim.SetFloat("horizontal", direction.x);
        anim.SetFloat("vertical", direction.y);
        movement.Set(direction.x, direction.y);

        while (time < duration)
        {
            anim.SetFloat("horizontal", direction.x);
            anim.SetFloat("vertical", direction.y);
            time += Time.deltaTime;
            float t = time / duration;
            transform.position = Vector2.Lerp(startingPos, location, t);
            yield return null;
        }

        transform.position = location;


        //anim.SetFloat("horizontal", 0);
        //anim.SetFloat("vertical", 0);
        //if (movement != Vector2.zero)
        //{
        //    anim.SetFloat("lasthorizontal", 0);
        //    anim.SetFloat("lastvertical", 0);
        //}

        //movement.Set(0, 0);
        //body.linearVelocity = Vector2.zero;

        //anim.SetFloat("horizontal", movement.x);
        //anim.SetFloat("vertical", movement.y);
        //if (movement != Vector2.zero)
        //{
        //    anim.SetFloat("lasthorizontal", movement.x);
        //    anim.SetFloat("lastvertical", movement.y);
        //}

        switch (direction)
        {
            case Vector2 v when v.x == 1 && v.y == 0:
                anim.Play("idleright");
                break;

            case Vector2 v when v.x == 0 && v.y == 1:
                anim.Play("idleup");
                break;
        }
    }

    //private void OnCollisionStay2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == "Enemy")
    //    {
    //        Debug.Log("DIE");
    //        if (state == State.Standard || state == State.Sword)
    //        {
    //            if (iFrames == false)
    //            {
    //                StartCoroutine(Knockback(collision.gameObject.transform.position));
    //                StartCoroutine(IFrames());
    //            }
    //        }
    //    }
    //}

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.tag == "Enemy")
    //    {
    //        Debug.Log("DIE");
    //        if (state == State.Standard || state == State.Sword)
    //        {
    //            if (iFrames == false)
    //            {
    //                StartCoroutine(Knockback(collision.gameObject.transform.position));
    //                StartCoroutine(IFrames());
    //            }
    //        }
    //    }
    //}

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Inspect" && state != State.NoMove)
        {
            inspectIcon.enabled = true;

            if (collision.gameObject.GetComponent("InteractableObject") as InteractableObject != null)
            {
                closestObject = collision.gameObject.GetComponent<InteractableObject>();
                if (collision.gameObject.GetComponent<InteractableObject>().checker == false)
                {
                    inspectIcon.color = Color.white;
                }
                else
                {
                    inspectIcon.color = dimmedColor;
                }
            }
        }

        if (collision.gameObject.tag == "Developer" && state != State.NoMove)
        {
            inspectIcon.enabled = true;
            closestDeveloper = collision.gameObject.GetComponent<DeveloperTalk>();

            if (collision.gameObject.GetComponent<DeveloperTalk>().checker == false)
            {
                inspectIcon.color = Color.white;
            }
            else
            {
                inspectIcon.color = dimmedColor;
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Inspect")
        {
            inspectIcon.enabled = false;
            closestObject = null;
        }

        if (collision.gameObject.tag == "Developer")
        {
            inspectIcon.enabled = false;
            closestDeveloper = null;
        }
    }

    public IEnumerator IFrames()
    {
        iFrames = true;
        Physics2D.IgnoreLayerCollision(6, 8);

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
        Physics2D.IgnoreLayerCollision(6, 8, false);
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }

    public IEnumerator Knockback(Vector2 damagePosition)
    {
        health -= 1;

        if (health > 0)
        {
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
            state = State.Dead;
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            Explosion newExplosion = Instantiate(explosion);
            newExplosion.transform.position = transform.position;
            //boxCollider.enabled = false;
            StartCoroutine(DeathScreen.instance.OnDeath());
        }

    }

    public IEnumerator ScreenTransition(string direction)
    {
        state = State.ScreenTrans;
        float time = 0;
        Vector3 startingPos = transform.position;

        Vector3 endingPos;

        switch (direction)
        {
            case "up":
                endingPos = new Vector3(startingPos.x + 0, startingPos.y + screenMoveAmount, 0);
                break;
            case "down":
                endingPos = new Vector3(startingPos.x + 0, startingPos.y - screenMoveAmount, 0);
                break;
            case "right":
                endingPos = new Vector3(startingPos.x + screenMoveAmount, startingPos.y + 0, 0);
                break;
            case "left":
                endingPos = new Vector3(startingPos.x - screenMoveAmount, startingPos.y + 0, 0);
                break;

            default:
                endingPos = new Vector3(0, 0, 0);
                break;
        }

        while (time < screenDuration)
        {
            time += Time.deltaTime;
            float t = time / screenDuration;
            transform.position = Vector3.Lerp(startingPos, endingPos, t);
            yield return null;
        }
        transform.position = endingPos;
        state = State.Standard;
    }
}
