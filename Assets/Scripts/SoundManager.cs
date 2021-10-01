using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip ButtonClickSound;
    public AudioClip NiceCommentClickSound;
    public AudioClip BadCommentClickSound;
    public AudioClip PowerupClickSound;

    public AudioClip HypeSound;
    public AudioClip LoseSound;
    public AudioClip WinSound;

    public void PlaySound(AudioClip sound, int times = 1)
    {
        StartCoroutine(playSoundCoroutine(sound, times));
        
    }

    IEnumerator playSoundCoroutine(AudioClip sound, int times)
    {
        for(int i = 0; i < times; ++i)
        {
            transform.GetComponent<AudioSource>().PlayOneShot(sound);
            yield return new WaitForSeconds(sound.length);
        }
        
    }
}
