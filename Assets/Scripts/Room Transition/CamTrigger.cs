using UnityEngine;

public class CamTrigger : MonoBehaviour
{
    public string direction;
    public CameraController mainCamera;
    public Player player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = Player.FindAnyObjectByType<Player>();
        mainCamera = CameraController.FindAnyObjectByType<CameraController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerBounds" && player.state != Player.State.ScreenTrans)
        {
            Debug.Log("trans");
            //mainCamera.CameraTransition(direction);
            //player.ScreenTransition(direction);
            StartCoroutine(mainCamera.CameraTransition(direction));
            StartCoroutine(player.ScreenTransition(direction));
        }
    }
}
