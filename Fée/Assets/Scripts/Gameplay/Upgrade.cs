using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    public delegate void OnUpgradeAction(int id);
    public static OnUpgradeAction OnUpgradePurcharsed;

    [Header("Assign Scriptable Object")]
    public UpgradeSO data;

    [Header("General Setttings")]
    public Text nameText;
    public Text levelText;
    public Text costText;
    public Text differenceText;
    public Color unavailableColor;

    [Header("Button Sprite Setttings")]
    public Button button;
    private UIAnimation animation;

    [Header("Check Current Setttings")]
    public Color currentColor;
    public int id;
    public int currentLevel;
    public bool isUnlocked;
    public int maxLevel;
    
    public bool CheckNextLevel()
    {
        if(currentLevel + 1 < data.amountPerLevel.Length)
        {
            return true;
        }

        return false;
    }

    public void SaveData()
    {
        data.isUnlocked = isUnlocked;
    }

    public void DeleteData()
    {
        data.isUnlocked = false;
    }

    public void ResetData()
    {
        currentLevel = 0;
        data.currentLevel = currentLevel;
    }

    public void BuyUpgrade()
    {
        animation.ExecuteCurves();
        UpgradeSystem.Get().BuyUpgrade(id);
        if(OnUpgradePurcharsed != null)
        {
            OnUpgradePurcharsed(id);
        }
    }

    public float GetCurrentAmount()
    {
        if(data.amountPerLevel.Length > 0)
        {
            return data.amountPerLevel[currentLevel];
        }

        return 0;
    }

    public void AssignData()
    {
        id = data.id;
        animation = GetComponent<UIAnimation>();
        button = GetComponentInChildren<Button>();
        nameText = transform.GetChild(1).GetChild(0).GetComponent<Text>();
        levelText = transform.GetChild(1).GetChild(1).GetComponent<Text>();
        differenceText = transform.GetChild(1).GetChild(2).GetComponent<Text>();
        costText = transform.GetChild(1).GetChild(3).GetComponent<Text>();
        currentColor = Color.white;
        

        SpriteState state;

        button.image.sprite = data.buttonIdle;
        state.disabledSprite = data.buttonDisabled;
        state.pressedSprite = data.buttonPressed;

        button.spriteState = state;
        isUnlocked = data.isUnlocked;
        currentLevel = data.currentLevel;
        maxLevel = data.amountPerLevel.Length;

        UpgradeSystem.Get().AddUpgrade(this);

        button.onClick.AddListener(BuyUpgrade);

        if (OnUpgradePurcharsed != null)
        {
            OnUpgradePurcharsed(id);
        }
    }
}
