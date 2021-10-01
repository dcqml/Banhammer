using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    public Transform BlurQualityBar;
    public Transform BlurChat;
    public Transform BlurStreamDesc;
    public Transform BlurStreamImage;
    public Transform BlurPowerups;

    public Material BlurMaterial;
    public Sprite InterfaceSprite;

    public TextMeshProUGUI Label;
    public TextMeshProUGUI ButtonLabel;

    public Transform TutorialWindow;

    Transform[] focusBlur;
    string[] tutorialTexts;
    int[] windowPosition;

    int tutoIndex = 0;

    Player Player
    {
        get
        {
            return FindObjectOfType<Player>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        focusBlur = new Transform[] { null, null, BlurStreamDesc, BlurChat, BlurQualityBar, BlurPowerups, BlurPowerups, BlurPowerups, BlurPowerups, BlurPowerups, BlurStreamImage, null };
        tutorialTexts = new string[]
        {
            "Welcome to Streamtopia! If you are here, that means you are motivated to pass the Streaming Moderator test, how brave!",
            "If you manage to keep the stream chat under control for 3 minutes, you pass. But it won't be that easy...",
            "Here is the stream description. You can see how much time you spent managing the chat.",
            "Next is the chat, your battlefield! Simply click on a message to mute it. But be careful, don't mute nice comments!",
            "Here is the Stream Quality bar, it helps you keeping track on how bad (or well?) you are at keeping things under control.",
            "Fortunately, as a moderator, you have some tools to help you manage the flood of messages.",
            "Each bad comment muted increases the progress bar for each tool, and you can use them when the bar is full.",
            "The Slow button prevents people from spamming with a delay. It lasts 10 seconds, use it when the hype amps up!",
            "The Bot AI button calls a bot to help you making distinction between normal comments and bad ones with a color code and lasts 10 seconds. ",
            "The Sub Only button prevents non subscribed viewers from posting. The chat is very slow for 5 seconds.",
            "Finally, the stream video itself. There is a 10 seconds delay between the stream and the chat, so keep an eye on it to predict the hype in the chat!",
            "Good luck on your moderation test, we wish you the best of luck."
        };
        windowPosition = new int[] { 0, 0, 2, 1, 1, 2, 2, 2, 2, 2, 2, 0 };
        tutoIndex = 0;
        updateTuto(tutoIndex);
    }

    public void OnNextClick()
    {
        var sm = FindObjectOfType<SoundManager>();
        sm.PlaySound(sm.ButtonClickSound);
        if (tutoIndex == focusBlur.Length - 1)
        {
            var mm = FindObjectOfType<MenuManager>();
            if(mm != null) mm.PlayTutorial = false;
            Player.Retry();
        }
        else
        {
            tutoIndex++;
            updateTuto(tutoIndex);
            if (tutoIndex == focusBlur.Length - 1)
            {
                ButtonLabel.text = "Begin test";
            }
        }
    }

    void updateTuto(int index)
    {
        Label.text = tutorialTexts[index];
        var rt = TutorialWindow.GetComponent<RectTransform>();
        switch(windowPosition[index])
        {
            case 0:
                rt.anchorMin = new Vector2(0.5f, 0.5f);
                rt.anchorMax = new Vector2(0.5f, 0.5f);
                rt.pivot = new Vector2(0.5f, 0.5f);
                rt.anchoredPosition = new Vector3(0, 0, 0);
                break;
            case 1:
                rt.anchorMin = new Vector2(0, 0.5f);
                rt.anchorMax = new Vector2(0, 0.5f);
                rt.pivot = new Vector2(0, 0.5f);
                rt.anchoredPosition = new Vector3(30, 0, 0);
                break;
            case 2:
                rt.anchorMin = new Vector2(1, 0.5f);
                rt.anchorMax = new Vector2(1, 0.5f);
                rt.pivot = new Vector2(1, 0.5f);
                rt.anchoredPosition = new Vector3(-30, 0, 0);
                break;
        }
        
        foreach (Transform tr in focusBlur)
        {
            var test = tr != focusBlur[index];
            if (tr != null && test)
            {
                var img = tr.GetComponent<Image>();
                img.material = BlurMaterial;
                img.fillCenter = true;
                img.sprite = null;
            }
            if (tr!=null && !test)
            {
                var img = tr.GetComponent<Image>();
                img.sprite = InterfaceSprite;
                img.material = null;
                img.type = Image.Type.Sliced;
                img.fillCenter = false;
            }
        }
    }
}
