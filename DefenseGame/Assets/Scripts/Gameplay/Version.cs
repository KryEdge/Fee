using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Version : MonoBehaviour
{
    private Text versionText;

    // Start is called before the first frame update
    private void Start()
    {
        versionText = GetComponent<Text>();
        versionText.text = "v" + Application.version;
    }
}
