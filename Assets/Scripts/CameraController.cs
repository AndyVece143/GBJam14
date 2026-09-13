using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float duration;
    private Vector3 endingPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator CameraTransition(string direction)
    {
        float time = 0;
        Vector3 startingPos = transform.position;

        switch (direction)
        {
            case "up":
                endingPos = new Vector3(startingPos.x + 0, startingPos.y + 128, -10);
                break;
            case "down":
                endingPos = new Vector3(startingPos.x + 0, startingPos.y -128, -10);
                break;
            case "right":
                endingPos = new Vector3(startingPos.x + 160, startingPos.y + 0, -10);
                break;
            case "left":
                endingPos = new Vector3(startingPos.x -160, startingPos.y + 0, -10);
                break;

            default:
                endingPos = new Vector3(0, 0, -10);
                break;
        }

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            transform.position = Vector3.Lerp(startingPos, endingPos, t);
            yield return null;
        }
        transform.position = endingPos;

    }
}
