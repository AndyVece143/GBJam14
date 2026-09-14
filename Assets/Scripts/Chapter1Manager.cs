using GBTemplate;
using UnityEngine;

public class Chapter1Manager : MonoBehaviour
{
    public GBDisplayController displayController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        displayController.UpdateColorPalette(3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
