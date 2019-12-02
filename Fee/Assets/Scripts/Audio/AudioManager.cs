using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public Slider sfxSlider;
    public Slider musicSlider;
    public float musicVolume;
    public float sfxVolume;
    private int type1 = 1;

    void Start()
    {
        AkSoundEngine.GetRTPCValue("music_volume", gameObject, 0, out musicVolume, ref type1);
        AkSoundEngine.GetRTPCValue("sfx_volume", gameObject, 0, out sfxVolume, ref type1);

        musicSlider.value = musicVolume;
        sfxSlider.value = sfxVolume;
    }

    public void ChangeMusicVolume()
    {
        Debug.Log("Changin music volume to :" + musicSlider.value);
        AkSoundEngine.SetRTPCValue("music_volume", musicSlider.value);
    }

    public void ChangeSFXVolume()
    {
        Debug.Log("Changin sfx volume to :" + sfxSlider.value);
        AkSoundEngine.SetRTPCValue("sfx_volume", sfxSlider.value);
    }
}
