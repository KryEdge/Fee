using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFairies : MonoBehaviour
{
    public Text fairiesText;

    // Start is called before the first frame update
    void Start()
    {
        UpdateText();
    }

    public void UpdateText()
    {
        fairiesText.text = GameManager.Get().currentFairies + "/" + GameManager.Get().maxFairies;
    }
}
