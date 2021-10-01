using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        LeanTween.scale(TutorialAskDialog.gameObject, new Vector3(0, 0, 0), 0);
        TutorialAskDialog.gameObject.SetActive(false);
    }

    public bool PlayTutorial;
    public Transform TutorialAskDialog;

    public void Play()
    {
        TutorialAskDialog.gameObject.SetActive(true);
        
        LeanTween.scale(TutorialAskDialog.gameObject, new Vector3(1, 1, 1), 0.35f).setDelay(0.1f).setEase(LeanTweenType.easeInCubic);
        var sm = FindObjectOfType<SoundManager>();
        sm.PlaySound(sm.ButtonClickSound);
    }

    public void PlayConfirm(bool tutorial)
    {
        PlayTutorial = tutorial;
        var sm = FindObjectOfType<SoundManager>();
        sm.PlaySound(sm.ButtonClickSound);

        this.GetComponent<AudioSource>().Stop();


        //FindObjectOfType<SceneLoader>().LoadScene("GameScene");
        SceneManager.LoadScene("GameScene");
    }
}
