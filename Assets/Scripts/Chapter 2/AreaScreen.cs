using UnityEngine;

public class AreaScreen : MonoBehaviour
{
    public Pirate[] pirateList;
    public Player player;
    public Chapter2Manager manager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        manager = Chapter2Manager.FindAnyObjectByType<Chapter2Manager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChaseThePlayer()
    {
        manager.ChangeToChaseMusic();
        for (int i = 0; i < pirateList.Length; i++)
        {
            pirateList[i].BeginChasing();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            manager.ChangeToRegularMusic();
            Debug.Log("Goodbyte");
            for (int i = 0; i < pirateList.Length; i++)
            {
                pirateList[i].ResetPosition();
            }
        }
    }
}
