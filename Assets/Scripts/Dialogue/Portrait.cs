using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Portrait : MonoBehaviour
{
    public bool isActiveSpeaker;
    public Image image;
    public Animator anim;
    public int emotion;
    public float duration;
    public Vector3 tinySize;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        image = GetComponent<Image>();
        anim.SetBool("done", true);
    }

    public void ChangeEmotion(int i)
    {
        if (i != emotion)
        {
            StartCoroutine(SpinToWin(i));
        }
        else
        {
            //anim.SetBool("done", false);
            anim.SetInteger("emotion", i);
            emotion = i;
        }
    }

    public void DoneTalking()
    {
        anim.SetBool("done", true);
    }

    IEnumerator SpinToWin(int i)
    {
        Vector3 initialScale = transform.localScale;

        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            transform.localScale = Vector3.Lerp(initialScale, tinySize, t);
            yield return null;
        }
        transform.localScale = tinySize;
        anim.SetInteger("emotion", i);
        anim.SetBool("done", false);

        float newTime = 0;
        while (newTime < duration)
        {
            newTime += Time.deltaTime;
            float t = newTime / duration;

            transform.localScale = Vector3.Lerp(tinySize, initialScale, t);
            yield return null;
        }
        transform.localScale = initialScale;

        emotion = i;
    }
}