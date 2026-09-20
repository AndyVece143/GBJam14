using GBTemplate;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Disclaimer : MonoBehaviour
{
    public bool canPressButton = false;
    public GBDisplayController displayController;
    public Image jamSplashScreen;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        displayController.UpdateColorPalette(41);
        StartCoroutine(FadingStuff());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) && canPressButton == true)
        {
            LevelLoader.instance.LoadNextLevel("Title");
        }
    }

    IEnumerator FadingStuff()
    {
        yield return new WaitForSeconds(3);
        StartCoroutine(displayController.FadeToWhite(1));
        yield return new WaitForSeconds(1.1f);
        jamSplashScreen.gameObject.SetActive(false);
        StartCoroutine(displayController.FadeFromWhite(1));
        yield return new WaitForSeconds(1.1f);
        canPressButton = true;
    }
}
