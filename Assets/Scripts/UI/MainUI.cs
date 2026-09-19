using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{
    public Player player;
    public TextMeshProUGUI goldText;
    public Slider healthSlider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = Player.FindAnyObjectByType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateText();
        UpdateSlider();
    }

    void UpdateSlider()
    {
        healthSlider.value = player.health;
    }

    void UpdateText()
    {
        goldText.text = "Gold: " + player.gold;
    }
}
