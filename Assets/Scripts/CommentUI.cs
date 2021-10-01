using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CommentUI : MonoBehaviour, IPointerDownHandler
{
    public float BadCommentProb = 0.33f;
    public Transform UserPicture;

    Player Player
    {
        get
        {
            return FindObjectOfType<Player>();
        }
    }

    public GameObject CommentTextObject;

    bool nice;
    public bool Nice
    {
        get
        {
            return nice;
        }
    }

    List<string> _niceTexts;
    List<string> niceTexts
    {
        get
        {
            if (_niceTexts == null)
            {
                _niceTexts = LoadComments(true, 1.5f);
            }
            return _niceTexts;
        }
    }

    List<string> _badTexts;
    List<string> badTexts
    {
        get
        {
            if (_badTexts == null)
            {
                _badTexts = LoadComments(false, 1);
            }
            return _badTexts;
        }
    }

    List<string> _niceTextsHype;
    List<string> niceTextsHype
    {
        get
        {
            if (_niceTextsHype == null)
            {
                _niceTextsHype = LoadComments(true, 0.5f);
            }
            return _niceTextsHype;
        }
    }

    List<string> _niceTextsbrb;
    List<string> niceTextsBrb
    {
        get
        {
            if (_niceTextsbrb == null)
            {
                _niceTextsbrb = LoadComments(true, 2.5f);
            }
            return _niceTextsbrb;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SetupComment();
        LeanTween.scale(gameObject, new Vector3(0, 0, 0), 0);
        LeanTween.scale(gameObject, new Vector3(1, 1, 1), 0.25f).setEase(LeanTweenType.easeInOutElastic);
    }

    string[] userColors = new string[] { "#00a8ff", "#9c88ff", "#fbc531", "#4cd137", "#487eb0",
    "#0097e6", "#8c7ae6", "#e1b12c", "#44bd32", "#40739e", "#e84118", "#c23616", "#f5f6fa",
    "#7f8fa6", "#273c75", "#353b48", "#dcdde1", "#718093", "#192a56", "#2f3640" };

    void SetupComment()
    {
        System.Random rd = new System.Random();
        nice = rd.NextDouble() > BadCommentProb;
        string commentText = (nice && FindObjectOfType<ChatUI>().CommentSpeed < 1 ? niceTextsHype[rd.Next(0, niceTextsHype.Count)] :
            (nice && FindObjectOfType<ChatUI>().CommentSpeed > 2 ? niceTextsBrb[rd.Next(0, niceTextsBrb.Count)] : (nice ? niceTexts[rd.Next(0, niceTexts.Count)] : badTexts[rd.Next(0, badTexts.Count)])));
        CommentTextObject.GetComponent<TextMeshProUGUI>().text = commentText;
        Debug.LogError(commentText);
        if (Player.BotIAActivated) CommentTextObject.GetComponent<TextMeshProUGUI>().color = nice ? Color.green : Color.red;
        foreach (Transform child in UserPicture.transform)
        {
            if(ColorUtility.TryParseHtmlString(userColors[rd.Next(0, userColors.Length)], out Color color))
            {
                child.GetComponent<Image>().color = color;
            }
        }
    }

    List<string> LoadComments(bool nice, float speed)
    {
        string niceTexts = "niceTexts"; string niceTextsHype = "niceTextsHype";
        string niceTextsBrb = "niceTextsBrb";  string badTexts = "badTexts";
        string path = (nice && speed < 1 ? niceTextsHype :
            (nice && speed > 2 ? niceTextsBrb : (nice ? niceTexts : badTexts)));

        //Read the text from directly from the test.txt file
        var text = Resources.Load<TextAsset>(path).text;
        var texts = text.Split('\n').ToList();
        return texts;
    }

    public bool Muted = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        if(!Muted)
        {
            Action Mute = () =>
            {
                GetComponent<Button>().interactable = false;
                var txt = CommentTextObject.GetComponent<TextMeshProUGUI>();
                txt.color = Color.white;
                txt.text = "muted";
                txt.alpha = 0.2f;
            };
            if (nice)
            {
                Player.OnNiceCommentClicked(transform);
                Mute();
                LeanTween.moveLocalX(gameObject, 3f, 0.1f).setDelay(0.1f).setEase(LeanTweenType.easeShake).setLoopCount(4);

            }
            else
            {
                Player.OnBadCommentClicked(transform);
                Mute();
                Action f = () => LeanTween.scale(gameObject, new Vector3(1f, 1f, 1f), 0.15f).setEase(LeanTweenType.easeInOutBounce);
                LeanTween.scale(gameObject, new Vector3(0.9f, 0.9f, 0.9f), 0.15f).setEase(LeanTweenType.easeInOutBounce).setOnComplete(f);
            }
            Muted = true;
        }
        
    }

    List<string> LoadCommentsOld(bool nice)
    {
        string niceTexts = "niceTexts.txt"; string niceTextsHype = "niceTextsHype.txt"; string badTexts = "badTexts.txt";
        string path = "Assets/Resources/" + (nice && FindObjectOfType<ChatUI>().CommentSpeed < 1 ? niceTextsHype : (nice ? niceTexts : badTexts));

        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(path);
        List<string> lines = new List<string>();
        while (!reader.EndOfStream)
        {
            lines.Add(reader.ReadLine());
        }
        return lines;
    }
}
