using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILoadingScreen : MonoBehaviourSingleton<UILoadingScreen>
{
    public Text loadingText;
    public Slider loadingBar;

    public override void Awake()
    {
        base.Awake();
        gameObject.SetActive(false);
    }

    public void SetVisible(bool show)
    {
        gameObject.SetActive(show);
    }

    public void Update()
    {
        int loadingVal = (int)(LoaderManager.Get().loadingProgress * 100) + 1;
        loadingText.text = "LOADING LEVEL " + loadingVal + "%";
        loadingBar.value = loadingVal;
        if (LoaderManager.Get().loadingProgress >= 1)
        {
            LoaderManager.Get().loadingProgress = 0;
            LoaderManager.Get().timeLoading = 0;
            SetVisible(false);
        }
            
    }
}
