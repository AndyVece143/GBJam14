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
    }
    public State state;
    public SpriteRenderer inspectIcon;
    public InteractableObject closestObject;
    public Color dimmedColor;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();

        inspectIcon.enabled = false;
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
        }
    }

    private void Movement()
    {
        //float horizontalInput = Input.GetAxis("Horizontal");
        //float verticalInput = Input.GetAxis("Vertical");

        //body.linearVelocity = new Vector2(horizontalInput * speed, verticalInput * speed);

        movement.Set(InputManager.Movement.x, InputManager.Movement.y);
        body.linearVelocity = movement * speed;

        anim.SetFloat("horizontal", movement.x);
        anim.SetFloat("vertical", movement.y);

        if (movement != Vector2.zero)
        {
            anim.SetFloat("lasthorizontal", movement.x);
            anim.SetFloat("lastvertical", movement.y);
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
    }

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
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Inspect")
        {
            inspectIcon.enabled = false;
            closestObject = null;
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
