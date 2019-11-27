using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
[CreateAssetMenu(fileName = "Upgrade Scriptable Object", menuName = "Frank/Utils/Upgrade Scriptable Object" + "", order = 1)]
public class UpgradeSO : ScriptableObject
{
    [Header("General Setttings")]
    public string upgradeName;
    public float[] amountPerLevel;
    public int[] costPerLevel;
    public string unitType;

    [Header("Button Sprite Setttings")]
    public Sprite buttonIdle;
    public Sprite buttonPressed;
    public Sprite buttonDisabled;

    [Header("Check Current Setttings")]
    public int id;
    public int currentLevel;
    public bool isUnlocked;
}
