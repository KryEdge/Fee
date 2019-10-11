using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class UIVignette : MonoBehaviour
{
    public PostProcessVolume post;
    [Range(0,3)]
    public float fadeSpeed;
    public float maxFadeValue;
    public float minFadeValue;
    public bool isFadeOn;
    public bool switchTimer;

    private Vignette lowHealthMask;
    public float fadeValueTimer;
    // Start is called before the first frame update
    private void Start()
    {
        post.profile.TryGetSettings(out lowHealthMask);
    }

    // Update is called once per frame
    private void Update()
    {
        if (isFadeOn)
        {
            if (switchTimer)
            {
                fadeValueTimer -= Time.deltaTime * fadeSpeed;
            }
            else
            {
                fadeValueTimer += Time.deltaTime * fadeSpeed;
            }
            

            if (fadeValueTimer >= maxFadeValue)
            {
                switchTimer = true;
            }
            else if(fadeValueTimer <= minFadeValue)
            {
                switchTimer = false;
            }

            lowHealthMask.intensity.value = fadeValueTimer;
        }
    }

    public void SwitchMask()
    {
        lowHealthMask.enabled.value = !lowHealthMask.enabled.value;
        SwitchFade();
    }

    private void SwitchFade()
    {
        isFadeOn = !isFadeOn;
        if(!isFadeOn)
        {
            fadeValueTimer = minFadeValue;
            switchTimer = false;
        }
    }
}
