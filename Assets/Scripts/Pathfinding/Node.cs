using UnityEngine;
using System.Collections.Generic;


public class Node : MonoBehaviour
{
    public Node cameFrom;
    public List<Node> connections;
    [SerializeField] private LayerMask raycastLayers;
    public Vector2[] directions;

    public float gScore;
    public float hScore;

    void Start()
    {
        FindConnections();
    }

    private void FindConnections()
    {
        for (int i = 0; i < directions.Length; i++)
        {
            RaycastHit2D otherNode = Physics2D.Raycast((Vector2)transform.position + directions[i], directions[i].normalized, 16, raycastLayers);

            if (otherNode.collider != null)
            {
                if (otherNode.collider.CompareTag("Node"))
                {
                    Debug.Log("Sans");
                    connections.Add(otherNode.collider.gameObject.GetComponent<Node>());
                }
            }
        }
    }

    public float FScore()
    {
        return gScore + hScore;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        if (connections.Count > 0)
        {
            for (int i = 0; i < connections.Count; i++)
            {
                Gizmos.DrawLine(transform.position, connections[i].transform.position);
            }
        }
    }
}
