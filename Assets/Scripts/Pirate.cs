using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
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
    public Node currentNode;
    public List<Node> path;

    public AreaScreen areaScreen;
    public bool stationary;
    private Vector2 initialDirection;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialDirection = directionVector;
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        point1 = transform.position;

        player = Player.FindAnyObjectByType<Player>();

        if (stationary == false)
        {
            directionVector = new Vector2(1, 0);
            StartCoroutine(Waiting());
        }

        else
        {

            anim.SetFloat("lasthoriztontal", directionVector.x);
            anim.SetFloat("lastvertical", directionVector.y);
        }
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
        if (path.Count == 0)
        {
            path = AStarManager.instance.GeneratePath(currentNode, AStarManager.instance.FindNearestNode(player.transform.position));
        }

        if (path.Count > 0)
        {
            int x = 0;
            Vector2 directionVector = (new Vector2(path[x].transform.position.x, path[x].transform.position.y) - (Vector2)transform.position);
            directionVector = directionVector.normalized;

            body.linearVelocity = directionVector * speed;

            anim.SetFloat("horizontal", directionVector.x);
            anim.SetFloat("vertical", directionVector.y);

            if (Vector2.Distance(transform.position, path[x].transform.position) < 1)
            {
                currentNode = path[x];
                path.RemoveAt(x);
            }
        }
    }

    public void ResetPosition()
    {
        StopAllCoroutines();
        state = State.Wander;
        //aiPath.canMove = false;
        transform.position = point1;
        position2 = false;

        if (stationary == false)
        {
            directionVector = new Vector2(1, 0);
            path.Clear();
            StartCoroutine(Waiting());
        }
        else
        {
            movement.Set(0, 0);
            body.linearVelocity = Vector2.zero;
            anim.SetFloat("horizontal", movement.x);
            anim.SetFloat("vertical", movement.y);
            directionVector = initialDirection;
            path.Clear();
        }
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Node" && state == State.Wander)
        {
            currentNode = collision.gameObject.GetComponent<Node>();
        }
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
