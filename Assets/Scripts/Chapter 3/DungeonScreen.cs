using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonScreen : MonoBehaviour
{
    //public Pirate[] pirateList;
    //public Slime[] slimeList;

    public List<Pirate> pirateList;
    public List<Slime> slimeList;

    public List<LockedDoor> doorList;
    private bool enemiesDefeated = false;
    private bool slimesDefeated = false;
    private bool piratesDefeated = false;
    public AudioClip doorSound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!slimesDefeated)
        {
            CheckForSlimes();
        }
 
        if (!piratesDefeated)
        {
            CheckForPirates();
        }

        if (slimesDefeated && piratesDefeated && !enemiesDefeated)
        {
            for (int i = 0; i < doorList.Count; i++)
            {
                doorList[i].OpenTheDoor();
            }
            enemiesDefeated = true;
            SoundManager.instance.PlaySound(doorSound);
        }
    }

    private void CheckForSlimes()
    {
        if (slimeList.Count > 0)
        {
            for (int i = 0; i < slimeList.Count; i++)
            {
                if (slimeList[i] == null)
                {
                    slimeList.RemoveAt(i);
                }
            }
        }
        else
        {
            slimesDefeated = true;
        }
    }

    private void CheckForPirates()
    {
        if (pirateList.Count > 0)
        {
            for (int i = 0; i < pirateList.Count; i++)
            {
                if (pirateList[i] == null)
                {
                    pirateList.RemoveAt(i);
                }
            }
        }
        else
        {
            piratesDefeated = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StopAllCoroutines();
            StartCoroutine(AggroPirates());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (pirateList.Count > 0)
            {
                for (int i = 0; i < pirateList.Count; i++)
                {
                    pirateList[i].ResetPosition();
                }
            }
        }
    }

    IEnumerator AggroPirates()
    {
        yield return new WaitForSeconds(1);

        for (int i = 0; i < pirateList.Count; i++)
        {
            pirateList[i].BeginChasing();
        }
    }
}
