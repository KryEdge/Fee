using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITowers : MonoBehaviour
{
    public Text towersText;

    // Start is called before the first frame update
    void Start()
    {
        UpdateText();
    }

    public void UpdateText()
    {
        towersText.text = TurretSpawner.Get().spawnedTurrets.Count + "/" + TurretSpawner.Get().maxTurrets;
    }
}
