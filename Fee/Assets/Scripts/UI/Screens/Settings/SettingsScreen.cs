using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsScreen : MonoBehaviour
{
    //screen.fullScreen
    public Button fullscreenButton;
    public Sprite pressedButton;
    public Sprite notPressedButton;

    //public bool windowed;
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
            if(resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                dropdown.value = i;
            }
        }

        doOnce = true;

        if (fullscreenButton)
        {
            if (Screen.fullScreen)
            {
                //windowed = false;
                //fullscreenButton.image.color = Color.green;
                fullscreenButton.image.sprite = pressedButton;
                //fullscreenButton.isOn = true;
            }
            else
            {
                //windowed = true;
                //fullscreenButton.image.color = Color.white;
                fullscreenButton.image.sprite = notPressedButton;
                //fullscreenButton.isOn = false;
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

            Screen.SetResolution(width, height, Screen.fullScreen);
        }
    }

    private void SwitchFullscreen()
    {
        //windowed = !windowed;
        int width = resolutions[dropdown.value].width;
        int height = resolutions[dropdown.value].height;

        Screen.SetResolution(width, height, Screen.fullScreen);
    }

    public void SettingsSwitchFullscreen()
    {
        if (!Screen.fullScreen)
        {
            fullscreenButton.image.sprite = pressedButton;
        }
        else
        {

            fullscreenButton.image.sprite = notPressedButton;
        }

        Screen.fullScreen = !Screen.fullScreen;
    }
}
