using GBTemplate;
using System.Collections;
using UnityEngine;

public class DoorTransition : MonoBehaviour
{
    public Vector2 playerTeleportPoint;
    public Player player;
    public GBDisplayController displayController;
    public CameraController mainCamera;
    public Vector3 cameraTeleportPoint;
    public int paletteColor;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        displayController = GBDisplayController.FindAnyObjectByType<GBDisplayController>();
        player = Player.FindAnyObjectByType<Player>();
        mainCamera = CameraController.FindAnyObjectByType<CameraController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && player.state != Player.State.NoMove)
        {
            player.StopMoving();
            StartCoroutine(TeleportTime());
        }
    }

    IEnumerator TeleportTime()
    {
        StartCoroutine(displayController.FadeToWhite(1));
        yield return new WaitForSeconds(1.1f);
        player.transform.position = playerTeleportPoint;
        mainCamera.transform.position = cameraTeleportPoint;
        displayController.UpdateColorPalette(paletteColor);
        StartCoroutine(displayController.FadeFromWhite(1));
        yield return new WaitForSeconds(1);
        player.StartMoving();
    }
}
