using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManagerTutorial : MonoBehaviour
{
    [Header("General Settings")]
    public GameObject mainCanvas;
    public GameObject backgroundCanvas;

    // Start is called before the first frame update
    private void Start()
    {
        mainCanvas.SetActive(true);
        backgroundCanvas.SetActive(false);
    }

    public void SwitchCanvas()
    {
        mainCanvas.SetActive(false);
        backgroundCanvas.SetActive(true);
    }
}
