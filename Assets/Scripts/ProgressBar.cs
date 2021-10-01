using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

//[ExecuteInEditMode()]
public class ProgressBar : MonoBehaviour
{
    //[MenuItem("GameObject/UI/Linear Progress Bar")]
    //public static void AddLinearProgressBar()
    //{
    //    GameObject obj = Instantiate(Resources.Load<GameObject>("UI/ProgressBar"));
    //    if (Selection.activeGameObject != null) obj.transform.SetParent(Selection.activeGameObject.transform, false);
    //}

    //[MenuItem("GameObject/UI/Radial Progress Bar")]
    //public static void AddRadialProgressBar()
    //{
    //    GameObject obj = Instantiate(Resources.Load<GameObject>("UI/RadialProgressBar"));
    //    obj.transform.SetParent(Selection.activeGameObject.transform, false);
    //}

    [Header("Bar progression intervals")]
    public int Minimum;
    public int Maximum;
    public int Current;

    [Space]
    [Header("Progress bar related objects")]
    public Image Mask;
    public Image Fill;
    public Color Color;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GetCurrentFill();
    }

    void GetCurrentFill()
    {
        float currentOffset = Current - Minimum;
        float maximumOffset = Maximum - Minimum;
        float fillAmount = currentOffset / maximumOffset;
        Mask.fillAmount = fillAmount;
        Fill.color = Color;
    }
}
