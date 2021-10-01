using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerBar : MonoBehaviour
{
    Player Player
    {
        get
        {
            return FindObjectOfType<Player>();
        }
    }

    public Button ActivationButton;

    public int Power;

    enum PowerType
    {
        Slow = 0,
        BotIA = 1,
        SubOnly = 2
    }

    int points;
    public int Points
    {
        get
        {
            return points;
        }
        set
        {
            var maxi = this.GetComponent<ProgressBar>().Maximum;
            points = value;
            if (points > maxi) points = maxi;
            transform.GetComponent<ProgressBar>().Current = points;
            ActivationButton.interactable = Points >= transform.GetComponent<ProgressBar>().Maximum;
        }
    }

    private void Update()
    {
        if(ActivationButton.interactable && !LeanTween.isTweening(gameObject))
        {
            LeanTween.scale(gameObject, new Vector3(1.2f, 1.2f, 1.2f), 0.5f).setLoopPingPong();
        }
        if(LeanTween.isTweening(gameObject))
        {
            LeanTween.cancel(gameObject);
        }
    }

    public void ActivatePower()
    {
        var sm = FindObjectOfType<SoundManager>();
        sm.PlaySound(sm.PowerupClickSound);
        switch ((PowerType)Power)
        {
            case PowerType.Slow:
                StartCoroutine(SlowChat(10.0f));
                break;
            case PowerType.BotIA:
                StartCoroutine(BotIA(10.0f));
                break;
            case PowerType.SubOnly:
                StartCoroutine(SubOnly(10.0f));
                break;
            default:
                break;
        }
    }

    IEnumerator SlowChat(float duration)
    {
        while(Points > 0)
        {
            Player.SlowActivated = true;
            Points -= (int)Math.Round(transform.GetComponent<ProgressBar>().Maximum / (2 * duration));
            yield return new WaitForSeconds(.5f);
        }
        Player.SlowActivated = false;
        yield return null;
    }

    IEnumerator BotIA(float duration)
    {
        while (Points > 0)
        {
            Player.BotIAActivated = true;
            Points -= (int)Math.Round(transform.GetComponent<ProgressBar>().Maximum / (2 * duration));
            yield return new WaitForSeconds(.5f);
        }
        Player.BotIAActivated = false;
        yield return null;
    }

    IEnumerator SubOnly(float duration)
    {
        while (Points > 0)
        {
            Player.SubOnlyActivated = true;
            Points -= (int)Math.Round(transform.GetComponent<ProgressBar>().Maximum / (2 * duration));
            yield return new WaitForSeconds(.5f);
        }
        Player.SubOnlyActivated = false;
        yield return null;
    }
}
