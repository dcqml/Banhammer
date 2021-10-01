using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class ChatUI : MonoBehaviour
{
    public Transform CommentUI;
    public Transform Content;

    public TextMeshProUGUI CommentSpeedText;

    public float MinCommentSpeed = 0.15f;
    public float SpeedMultiplier = 1.0f;
    public float SpeedBlur = 0.5f;
    public float Delay = 10.0f;
    public float ChatLevelTime;
    public AnimationCurve CommentSpeedCurve;

    Player Player
    {
        get
        {
            return FindObjectOfType<Player>();
        }
    }

    public DateTime StartTime = DateTime.Now;
    void Start()
    {
        StartTime = DateTime.Now;
        if(!Player.InTutorial)
        {
            StartCoroutine(ManageChat());
        }
    }

    bool gameSuccessCalled = false;

    void Update()
    {
        float time = (float)(DateTime.Now - StartTime).TotalSeconds;
        if (time > ChatLevelTime + 1 && !gameSuccessCalled && !Player.InTutorial)
        {
            gameSuccessCalled = true;
            Player.GameSuccess();
        }
    }

    public float CommentSpeed
    {
        get
        {
            float time = (float)(DateTime.Now - StartTime).TotalSeconds / ChatLevelTime;
            var curveSpeed = CommentSpeedCurve.Evaluate(time);
            Debug.Log($"Comment speed : {curveSpeed}");
            CommentSpeedText.text = curveSpeed.ToString();
            return curveSpeed;
        }
    }

    bool inHype = false;

    public float StreamSpeed
    {
        get
        {
            float time = (float)((DateTime.Now - StartTime).TotalSeconds + Delay) / ChatLevelTime;
            var curveSpeed = CommentSpeedCurve.Evaluate(time);
            if(curveSpeed < 0.5f && !inHype)
            {
                var sm = FindObjectOfType<SoundManager>();
                sm.PlaySound(sm.HypeSound, 3);
                inHype = true;
            }
            else
            {
                if(curveSpeed > 0.7f)
                {
                    inHype = false;
                }
            }
            //Debug.Log($"Stream speed in 5 secs: {curveSpeed}");
            return curveSpeed;
        }
    }

    float NextCommentSpeed
    {
        get
        {
            var rd = new System.Random();
            SpeedMultiplier = Player.SlowActivated ? 2.0f : 1.0f;
            var commentSpeed = (float)Math.Max(MinCommentSpeed, ((0.75 * CommentSpeed) + (float)(rd.NextDouble() * 0.5f * CommentSpeed))) * SpeedMultiplier;
            return Player.SubOnlyActivated ? 3.0f : commentSpeed;
        }
    }

    IEnumerator ManageChat()
    {
        while(Player.Quality > 0)
        {
            SpawnComment();
            var speed = NextCommentSpeed;
            yield return new WaitForSeconds(speed);
        }
    }

    void SpawnComment()
    {
        var newComment = UnityEngine.Object.Instantiate(CommentUI);
        newComment.SetParent(Content);
    }
}
