using System.Collections;
using System.IO;
using UnityEngine;

public class Pirate : MonoBehaviour
{
    public float speed;
    public Rigidbody2D body;
    public BoxCollider2D boxCollider;
    public Animator anim;
    private Vector2 movement;

    public enum State
    {
        Wander,
        Chase,
    }
    public State state;
    //public string direction;
    public Vector2 directionVector;
    public Vector2 point1;
    public Vector2 point2;

    public float moveTimer;
    public float stayStillTimer;
    public bool position2 = false;
    public bool sawPlayer = false;

    public Transform ledgeDetector;
    public float sightDistance;
    [SerializeField] private LayerMask raycastLayers;
    public Player player;


    public AreaScreen areaScreen;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        point1 = transform.position;
        directionVector = new Vector2(1, 0);
        player = Player.FindAnyObjectByType<Player>();

        StartCoroutine(Waiting());
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D detection = Physics2D.Raycast(ledgeDetector.position, directionVector, sightDistance, raycastLayers);

        if (detection.collider != null)
        {
            Debug.DrawLine(ledgeDetector.position, detection.point, Color.red);
            if (detection.collider.CompareTag("Player") && state != State.Chase)
            {
                Debug.Log("Found you");
                areaScreen.ChaseThePlayer();
            }
        }

        switch (state)
        {
            case State.Chase:
                ChaseMovement();
                break;
            case State.Wander:
                break;
        }
    }

    public void BeginChasing()
    {
        StopAllCoroutines();
        //destination.target = player.transform;
        //aiPath.canMove = true;
        state = State.Chase;
    }

    private void ChaseMovement()
    {
        //aiPath.canMove = true;
        //anim.SetFloat("horizontal", aiPath.desiredVelocity.x);
        //anim.SetFloat("vertical", aiPath.desiredVelocity.y);
    }

    public void ResetPosition()
    {
        StopAllCoroutines();
        state = State.Wander;
        //aiPath.canMove = false;
        transform.position = point1;
        position2 = false;
        directionVector = new Vector2(1, 0);
        StartCoroutine(Waiting());
    }

    private void GetDirection()
    {
        ////Left
        //if (anim.GetFloat("lasthorizontal") == -1 && anim.GetFloat("lastvertical") == 0)
        //{
        //    //Debug.Log("Left");
        //    direction = "left";
        //    directionVector = new Vector2(0, -1);
        //}

        ////Right
        //if (anim.GetFloat("lasthorizontal") == 1 && anim.GetFloat("lastvertical") == 0)
        //{
        //    //Debug.Log("Right");
        //    direction = "right";
        //}

        ////Up
        //if (anim.GetFloat("lasthorizontal") == 0 && anim.GetFloat("lastvertical") == 1)
        //{
        //    //Debug.Log("Up");
        //    direction = "up";
        //}

        ////Down
        //if (anim.GetFloat("lasthorizontal") == 0 && anim.GetFloat("lastvertical") == -1)
        //{
        //    //Debug.Log("Down");
        //    direction = "down";
        //}

        ////UpRight
        //if (anim.GetFloat("lasthorizontal") > 0 && anim.GetFloat("lasthorizontal") == anim.GetFloat("lastvertical"))
        //{
        //    //Debug.Log("Up Right");
        //    direction = "up";
        //}

        ////DownLeft
        //if (anim.GetFloat("lasthorizontal") < 0 && anim.GetFloat("lasthorizontal") == anim.GetFloat("lastvertical"))
        //{
        //    //Debug.Log("Down Left");
        //    direction = "down";
        //}

        ////Down Right
        //if (anim.GetFloat("lasthorizontal") > 0 && anim.GetFloat("lasthorizontal") + anim.GetFloat("lastvertical") == 0)
        //{
        //    //Debug.Log("Down Right");
        //    direction = "down";
        //}

        ////UpLeft
        //if (anim.GetFloat("lasthorizontal") < 0 && anim.GetFloat("lasthorizontal") + anim.GetFloat("lastvertical") == 0)
        //{
        //    //Debug.Log("Up Left");
        //    direction = "up";
        //}
    }

    public IEnumerator GoToPlace(Vector2 location, float duration)
    {
        float time = 0;
        Vector2 startingPos = transform.position;

        Vector2 direction = location - startingPos;

        direction = direction.normalized;

        directionVector = direction;

        anim.SetFloat("horizontal", direction.x);
        anim.SetFloat("vertical", direction.y);

        anim.SetFloat("lasthoriztontal", direction.x);
        anim.SetFloat("lastvertical", direction.y);
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

        StartCoroutine(Waiting());
    }

    public IEnumerator Waiting()
    {
        movement.Set(0, 0);
        body.linearVelocity = Vector2.zero;
        anim.SetFloat("horizontal", movement.x);
        anim.SetFloat("vertical", movement.y);
        //if (movement != Vector2.zero)
        //{
        //    anim.SetFloat("lasthoriztontal", movement.x);
        //    anim.SetFloat("lastvertical", movement.y);
        //}
        yield return new WaitForSeconds(stayStillTimer);


        if (position2 == false)
        {
            StartCoroutine(GoToPlace(point2, moveTimer));
            position2 = true;
        }

        else
        {
            StartCoroutine(GoToPlace(point1, moveTimer));
            position2 = false;
        }
    }
}
