using System.Collections;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private Animator anim;
    public AudioClip sound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        StartCoroutine(Explode());
    }

    IEnumerator Explode()
    {
        anim.Play("stars");
        SoundManager.instance.PlaySound(sound);
        yield return null;
        float duration = anim.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }
}
