using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsScreen : MonoBehaviour
{
    //screen.fullScreen
    public Toggle fullscreenButton;

    public bool windowed;
    public Resolution[] resolutions;
    public Dropdown dropdown;

    private bool doOnce;
    // Start is called before the first frame update
    void Start()
    {
        dropdown.options.Clear();
        resolutions = Screen.resolutions;

        for (int i = 0; i < resolutions.Length; i++)
        {
            Dropdown.OptionData data = new Dropdown.OptionData();
            Debug.Log(resolutions[i].width + "x" + resolutions[i].height + " : " + resolutions[i].refreshRate + " Hz");
            data.text = resolutions[i].width + "x" + resolutions[i].height + " : " + resolutions[i].refreshRate + " Hz";
            dropdown.options.Add(data);
        }

        for (int i = 0; i < resolutions.Length; i++)
        {
            if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                dropdown.value = i;
            }
        }

        doOnce = true;

        if (fullscreenButton)
        {
            if (Screen.fullScreen)
            {
                fullscreenButton.image.color = Color.green;
            }
            else
            {
                fullscreenButton.image.color = Color.white;
            }
        }
    }

    public void ChangeResolution()
    {
        if(doOnce)
        {
            Debug.Log("LAST RESOLUTION: " + Screen.currentResolution.width + "x" + Screen.currentResolution.height);
            int width = resolutions[dropdown.value].width;
            int height = resolutions[dropdown.value].height;

            Debug.Log("NEW RESOLUTION: " + width + "x" + height);

            Screen.SetResolution(width, height, windowed);
        }
    }

    private void SwitchFullscreen()
    {
        windowed = !windowed;
        int width = resolutions[dropdown.value].width;
        int height = resolutions[dropdown.value].height;

        Screen.SetResolution(width, height, windowed);
    }

    public void SettingsSwitchFullscreen()
    {
        SwitchFullscreen();

        if (Screen.fullScreen)
        {
            fullscreenButton.image.color = Color.green;
        }
        else
        {
            fullscreenButton.image.color = Color.white;
        }
    }
}
