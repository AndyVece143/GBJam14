using TMPro;
using UnityEngine;

public class MainUI : MonoBehaviour
{
    public Player player;
    public TextMeshProUGUI goldText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = Player.FindAnyObjectByType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateText();
    }

    void UpdateText()
    {
        goldText.text = "Gold: " + player.gold;
    }
}
