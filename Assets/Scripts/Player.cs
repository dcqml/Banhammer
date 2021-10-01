using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public AudioClip MainMusic;
    public AudioClip TutoMusic;

    public Transform StreamQualityBar;
    public Transform SlowBar;
    public Transform BotIABar;
    public Transform SubOnlyBar;
    public TextMeshProUGUI StreamTimeText;
    public Animator StreamImageAnimator;
    public Transform GameOverScreen;
    public TextMeshProUGUI GameOverScreenTimeText;
    public Transform GameSuccessScreen;
    DateTime startTime;

    public Transform TutorialUI;
    public Transform TutorialWindow;

    public bool SlowActivated = false;
    public bool BotIAActivated = false;
    public bool SubOnlyActivated = false;

    public bool InTutorial
    {
        get
        {
            var mm = FindObjectOfType<MenuManager>();
            if(mm != null)
            {
                return mm.PlayTutorial;
            }
            else
            {
                return false;
            }
            
        }
        set
        {
            var mm = FindObjectOfType<MenuManager>();
            if (mm != null)
            {
                mm.PlayTutorial = value;
            }
        }
    }

    int quality;
    public int Quality
    {
        get
        {
            return quality;
        }
        set
        {
            var streamBar = StreamQualityBar.GetComponent<ProgressBar>();
            quality = value;
            if (quality > streamBar.Maximum) quality = streamBar.Maximum;
            streamBar.Current = quality;
        }
    }

    private void Awake()
    {
        
        LeanTween.scale(TutorialWindow.gameObject, new Vector3(0, 0, 0), 0);
        LeanTween.scale(GameOverScreen.gameObject, new Vector3(0, 0, 0), 0);
        LeanTween.scale(GameSuccessScreen.gameObject, new Vector3(0, 0, 0), 0);
    }

    // Start is called before the first frame update
    void Start()
    {
        Quality = 500;
        SlowBar.GetComponent<ProgressBar>().Current = 0;
        BotIABar.GetComponent<ProgressBar>().Current = 0;
        SubOnlyBar.GetComponent<ProgressBar>().Current = 0;
        var source = GetComponent<AudioSource>();
        if (InTutorial)
        {
            source.clip = TutoMusic;
            source.Play();
            StartTutorial();
        }
        else
        {
            source.clip = MainMusic;
            source.Play();
            StartGame();
        }
    }

    void StartGame()
    {
        TutorialUI.gameObject.SetActive(false);
        Quality = 500;
        SlowBar.GetComponent<ProgressBar>().Current = 0;
        BotIABar.GetComponent<ProgressBar>().Current = 0;
        SubOnlyBar.GetComponent<ProgressBar>().Current = 0;
        Time.timeScale = 1;
        StartCoroutine(ManageStreamTime());
        StartCoroutine(ManageStreamImage());
    }

    void StartTutorial()
    {
        TutorialUI.gameObject.SetActive(true);
        LeanTween.scale(TutorialWindow.gameObject, new Vector3(1, 1, 1), 0.35f).setDelay(0.1f).setEase(LeanTweenType.easeInCubic);
    }

    private bool gameOverCalled = false;
    // Update is called once per frame
    void Update()
    {
        if (Quality <= 0 && !gameOverCalled)
        {
            gameOverCalled = true;
            GameOver();
        }
    }

    public void OnNiceCommentClicked(Transform comment)
    {
        var sm = FindObjectOfType<SoundManager>();
        sm.PlaySound(sm.NiceCommentClickSound);
        Quality -= 75;
    }

    public void OnBadCommentClicked(Transform comment)
    {
        var sm = FindObjectOfType<SoundManager>();
        sm.PlaySound(sm.BadCommentClickSound);
        Quality += 50;
        if(!SlowActivated) SlowBar.GetComponent<PowerBar>().Points += 10;
        if(!BotIAActivated) BotIABar.GetComponent<PowerBar>().Points += 10;
        if (!SubOnlyActivated) SubOnlyBar.GetComponent<PowerBar>().Points += 10;
    }

    IEnumerator ManageStreamTime()
    {
        while(Quality > 0)
        {
            var elapsed = DateTime.Now - FindObjectOfType<ChatUI>().StartTime;
            StreamTimeText.text = $"{Math.Floor(elapsed.TotalMinutes):00}:{(Math.Floor(elapsed.TotalSeconds) - 60* Math.Floor(elapsed.TotalMinutes)):00}";
            yield return new WaitForSeconds(1.0f);
        }
    }

    IEnumerator ManageStreamImage()
    {
        while(Quality > 0)
        {
            StreamImageAnimator.SetFloat("Speed", FindObjectOfType<ChatUI>().StreamSpeed);
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void GameOver()
    {
        var sm = FindObjectOfType<SoundManager>();
        sm.PlaySound(sm.LoseSound, 3);
        Action f = () => Time.timeScale = 0;
        var now = DateTime.Now;
        var elapsed = now - FindObjectOfType<ChatUI>().StartTime;
        GameOverScreenTimeText.text = $"{Math.Floor(elapsed.TotalMinutes):00}:{(Math.Floor(elapsed.TotalSeconds) - 60 * Math.Floor(elapsed.TotalMinutes)):00}";
        //GameOverScreenTimeText.text = $"{Math.Floor(elapsed.TotalMinutes):00}:{(Math.Floor(elapsed.TotalSeconds) - 60 * Math.Floor(elapsed.TotalMinutes)):00}";
        GameOverScreen.gameObject.SetActive(true);
        LeanTween.scale(GameOverScreen.gameObject, new Vector3(1, 1, 1), 0.35f).setDelay(0.1f).setEase(LeanTweenType.easeInCubic).setOnComplete(f);
    }

    public void GameSuccess()
    {
        var sm = FindObjectOfType<SoundManager>();
        sm.PlaySound(sm.WinSound, 2);
        Action f = () => Time.timeScale = 0;
        GameSuccessScreen.gameObject.SetActive(true);
        LeanTween.scale(GameSuccessScreen.gameObject, new Vector3(1, 1, 1), 0.35f).setDelay(0.1f).setEase(LeanTweenType.easeInCubic).setOnComplete(f);
    }

    public void Retry()
    {
        var sm = FindObjectOfType<SoundManager>();
        sm.PlaySound(sm.ButtonClickSound);
        var scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public Transform ModCertificate;
    public TextMeshProUGUI ModCertificateNameText;
    public TextMeshProUGUI ModName;
    public TextMeshProUGUI CertificateDate;

    public void ShowModCertificate(bool show)
    {
        ModCertificateNameText.text = ModName.text;
        ModCertificate.gameObject.SetActive(show);
        CertificateDate.text = DateTime.Today.ToString("dd/MM/yyyy");
    }
}
