using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class UIVignette : MonoBehaviour
{
    public PostProcessVolume post;
    [Range(0,3)]
    public float fadeSpeed;
    public float newWaveFadeSpeed;
    public float maxFadeValue;
    public float minFadeValue;
    public float maxFadeValueMask;
    public bool isFadeOn;
    public bool switchTimer;
    public bool isLooping;
    public bool lowHealthActivated;
    public int loopTimes;
    public ColorParameter lowHealthColor;
    public Color newWaveColor;
    public Image VignetteMask;

    public bool doOnce;

    [Header("Assign Variables")]
    private Vignette lowHealthMask;
    public float fadeValueTimer;
    // Start is called before the first frame update
    private void Start()
    {
        WaveSystem.OnStartWave += SetWaveColor;
        post.profile.TryGetSettings(out lowHealthMask);

        lowHealthMask.color.value = lowHealthColor;

        newWaveColor.a = 0;
        VignetteMask.color = newWaveColor;
        VignetteMask.gameObject.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        if (isFadeOn)
        {

            if(isLooping)
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
                    doOnce = false;
                }
                else if (fadeValueTimer <= minFadeValue)
                {
                    switchTimer = false;
                }

                lowHealthMask.intensity.value = fadeValueTimer;
            }
            else
            {
                if (loopTimes > 1)
                {
                    fadeValueTimer = 0;
                    loopTimes = 0;
                    isFadeOn = false;
                }

                if(switchTimer)
                {
                    fadeValueTimer -= Time.deltaTime * newWaveFadeSpeed;
                }
                else
                {
                    fadeValueTimer += Time.deltaTime * newWaveFadeSpeed;
                }

                if (fadeValueTimer >= maxFadeValueMask)
                {
                    switchTimer = true;
                    doOnce = false;
                }
                else if (fadeValueTimer <= minFadeValue)
                {
                    if (fadeValueTimer <= 0)
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

            newWaveColor.a = fadeValueTimer;
            VignetteMask.color = newWaveColor;
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
            VignetteMask.gameObject.SetActive(true);
        }
    }

    public void SetLowHealthColor()
    {
        isFadeOn = true;
        isLooping = true;
        lowHealthMask.enabled.value = false;
        lowHealthMask.color.value = lowHealthColor;
        lowHealthActivated = true;
        if(VignetteMask)
        {
            VignetteMask.enabled = false;
        }
    }
}
