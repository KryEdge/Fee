using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManagerMenu : MonoBehaviour
{
    [Header("General Settings")]
    public Button upgradesButton;

    // Start is called before the first frame update
    private void Start()
    {
        int goToTutorial = PlayerPrefs.GetInt("isFirstTimePlaying", 1);

        if (goToTutorial == 1)
        {
            upgradesButton.interactable = false;
            upgradesButton.image.color = Color.gray;
        }
        else if (goToTutorial == 0)
        {
            upgradesButton.interactable = true;
            upgradesButton.image.color = Color.white;
        }
    }
}
