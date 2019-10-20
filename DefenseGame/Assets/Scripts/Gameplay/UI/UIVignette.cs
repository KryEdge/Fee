using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class UIVignette : MonoBehaviour
{
    public PostProcessVolume post;
    [Range(0,3)]
    public float fadeSpeed;
    public float newWaveFadeSpeed;
    public float maxFadeValue;
    public float minFadeValue;
    public bool isFadeOn;
    public bool switchTimer;
    public bool isLooping;
    public bool lowHealthActivated;
    public int loopTimes;
    public ColorParameter lowHealthColor;
    public ColorParameter newWaveColor;

    public bool doOnce;

    [Header("Assign Variables")]
    private Vignette lowHealthMask;
    public float fadeValueTimer;
    // Start is called before the first frame update
    private void Start()
    {
        WaveSystem.OnStartWave += SetWaveColor;
        post.profile.TryGetSettings(out lowHealthMask);
        if(isLooping)
        {
            lowHealthMask.color.value = lowHealthColor;
        }
        else
        {
            lowHealthMask.color.value = newWaveColor;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (isFadeOn)
        {
            if(!isLooping)
            {
                if(loopTimes > 1)
                {
                    fadeValueTimer = 0;
                    loopTimes = 0;
                    isFadeOn = false;
                }
            }

            if (switchTimer)
            {
                if(isLooping)
                {
                    fadeValueTimer -= Time.deltaTime * fadeSpeed;
                }
                else
                {
                    fadeValueTimer -= Time.deltaTime * newWaveFadeSpeed;
                }
                
            }
            else
            {
                if (isLooping)
                {
                    fadeValueTimer += Time.deltaTime * fadeSpeed;
                }
                else
                {
                    fadeValueTimer += Time.deltaTime * newWaveFadeSpeed;
                }
            }
            

            if (fadeValueTimer >= maxFadeValue)
            {
                switchTimer = true;
                doOnce = false;
            }
            else if(fadeValueTimer <= minFadeValue)
            {
                if(isLooping)
                {
                    switchTimer = false;
                }
                else
                {
                    if(fadeValueTimer <= 0)
                    {
                        switchTimer = false;
                        if (!doOnce)
                        {
                            loopTimes++;
                            doOnce = true;
                        }
                    }
                }
               
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

    private void OnDestroy()
    {
        WaveSystem.OnStartWave -= SetWaveColor;
    }

    public void SetWaveColor()
    {
        if(!lowHealthActivated)
        {
            isFadeOn = true;
            isLooping = false;
            lowHealthMask.color.value = newWaveColor;
            lowHealthMask.enabled.value = true;
        }
    }

    public void SetLowHealthColor()
    {
        isFadeOn = false;
        isLooping = true;
        lowHealthMask.enabled.value = false;
        lowHealthMask.color.value = lowHealthColor;
        lowHealthActivated = true;
    }
}
