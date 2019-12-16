using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class AudioManager : MonoBehaviour
{
    [System.Serializable]
    public class VolumeOptions
    {
        public float sfxVol;
        public float musicVol;

        public string Serialize()
        {
            return JsonUtility.ToJson(this);
        }

        public void Deserialize(string json)
        {
            JsonUtility.FromJsonOverwrite(json, this);
        }
    }

    public Slider sfxSlider;
    public Slider musicSlider;
    public float musicVolume;
    public float sfxVolume;
    private int type1 = 1;
    private VolumeOptions data;
    private DataManager dm;

    void Start()
    {
        data = new VolumeOptions();
        dm = DataManager.Instance;
        string deserializedString;

        if(dm.loadData("soundData.txt",out deserializedString))
        {
            data.Deserialize(deserializedString);

            musicVolume = data.musicVol;
            sfxVolume = data.sfxVol;
        }
        else
        {
            musicVolume = 70;
            sfxVolume = 75;
        }

        if(musicSlider)
        {
            musicSlider.value = musicVolume;
            sfxSlider.value = sfxVolume;

            AkSoundEngine.SetRTPCValue("music_volume", musicSlider.value);
            AkSoundEngine.SetRTPCValue("sfx_volume", sfxSlider.value);
        }
        else
        {
            AkSoundEngine.SetRTPCValue("music_volume", musicVolume);
            AkSoundEngine.SetRTPCValue("sfx_volume", sfxVolume);
        }
        

        /*musicSlider.value = musicVolume;
        sfxSlider.value = sfxVolume;*/

        data.sfxVol = sfxVolume;
        data.musicVol = musicVolume;

        dm.saveData("soundData.txt", data.Serialize());
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

    public void ApplyChanges()
    {
        data.sfxVol = sfxSlider.value;
        data.musicVol = musicSlider.value;

        dm.saveData("soundData.txt", data.Serialize());
    }

    private void OnDestroy()
    {
        if(data != null)
        {
            if(sfxSlider)
            {
                data.sfxVol = sfxSlider.value;
            }

            if(musicSlider)
            {
                data.musicVol = musicSlider.value;
            }

            dm.saveData("soundData.txt", data.Serialize());
        }
    }
}
