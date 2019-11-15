using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class UIAnimation : MonoBehaviour
{
    [System.Serializable]
    public class gradientObject
    {
        public int objectIndex;
        public int gradientIndex;
    }

    [Header("Assign UI Elements")]
    public GameObject[] elements;

    [Header("General Settings")]
    public float animationTime;
    [Tooltip("Adds a key of your choice to test the curves.")]
    public bool testKeyActive;
    public KeyCode testKey;

    [Header("Gradient Settings")]
    [Tooltip("One Minimum gradient.")]
    public Gradient[] gradients;
    public bool sameGradientForAll;
    [Tooltip("If 'sameGradientForAll' is set to false, configure here which gradients will affect the element you add , Left side is for gradient element and right side is for object element.")]
    //public int[] gradientElementsToAffect;
    public gradientObject[] gradientElementsToAffect;

    [Header("Curve Settings")]
    public AnimationCurve curve;
    public AnimationCurve curveColor;

    private RectTransform[] rectTransforms;
    private Image[] materials;

    // Start is called before the first frame update
    void Start()
    {
        rectTransforms = new RectTransform[elements.Length];
        materials = new Image[elements.Length];

        for (int i = 0; i < rectTransforms.Length; i++)
        {
            rectTransforms[i] = elements[i].GetComponent<RectTransform>();
            materials[i] = elements[i].GetComponent<Image>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(testKeyActive)
        {
            if (Input.GetKeyDown(testKey))
            {
                ExecuteScaleCurve();
                ExecuteGradientCurve();
            }
        }
    }

    public void ExecuteScaleCurve()
    {
        StartCoroutine(ScaleCurve());
    }

    public void ExecuteGradientCurve()
    {
        StartCoroutine(GradientCurve());
    }

    public void ExecuteCurves()
    {
        StartCoroutine(ScaleCurve());
        StartCoroutine(GradientCurve());
    }

    public void ChangeGradientElement(int objectElement, int gradientElement)
    {
        for (int i = 0; i < gradientElementsToAffect.Length; i++)
        {
            if(objectElement == gradientElementsToAffect[i].objectIndex)
            {
                gradientElementsToAffect[i].gradientIndex = gradientElement;
            }
        }
    }

    IEnumerator ScaleCurve()
    {
        float t = 0;

        while (t <= animationTime)
        {
            t += Time.deltaTime;

            float eval = curve.Evaluate(t / animationTime);

            for (int i = 0; i < rectTransforms.Length; i++)
            {
                rectTransforms[i].localScale = Vector3.one * eval;
            }
            yield return null;
        }
    }

    IEnumerator GradientCurve()
    {
        float t = 0;

        while (t <= animationTime)
        {
            t += Time.deltaTime;

            float eval = curveColor.Evaluate(t / animationTime);


            if(sameGradientForAll)
            {
                for (int i = 0; i < materials.Length; i++)
                {
                    materials[i].color = gradients[0].Evaluate(eval);
                }
            }
            else
            {
                for (int i = 0; i < gradientElementsToAffect.Length; i++)
                {
                    materials[gradientElementsToAffect[i].objectIndex].color = gradients[gradientElementsToAffect[i].gradientIndex].Evaluate(eval);
                }
            }

            yield return null;
        }
    }
}
