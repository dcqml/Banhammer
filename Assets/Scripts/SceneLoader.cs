using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    //private AssetBundle assetBundle;
    //private string[] scenePaths;
    //// Start is called before the first frame update
    //void Start()
    //{
    //    assetBundle = AssetBundle.LoadFromFile("Assets/Scenes");
    //    scenePaths = assetBundle.GetAllScenePaths();
    //}
    public Animator Transition;
    public float TransitionTime = 1.0f;

    public void LoadScene(string scenePath)
    {
        //SceneManager.LoadScene(scenePaths.First(x => x == scenePath));
        StartCoroutine(loadScene(scenePath));
    }

    protected IEnumerator loadScene(string scenePath)
    {
        Transition.SetTrigger("Start");
        yield return new WaitForSeconds(TransitionTime);
        SceneManager.LoadScene(scenePath);
    }
}
