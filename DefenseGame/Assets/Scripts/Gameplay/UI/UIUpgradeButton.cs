using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUpgradeButton : MonoBehaviour
{
    public enum UpgradeType
    {
        maxFairies,
        maxTowers,
        towersFireRate,
        allTypes
    }

    public UpgradeType type;
    public UIManager manager;

    private Button button;
    //private UpgradeSystem upgrades;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        //upgrades = UpgradeSystem.Get();
        button.onClick.AddListener(GetUpgrade);
    }

    public void GetUpgrade()
    {
        switch (type)
        {
            case UpgradeType.maxFairies:
                manager.UpgradeBuyMaxFaires();
                break;
            case UpgradeType.maxTowers:
                manager.UpgradeBuyMaxTowers();
                break;
            case UpgradeType.towersFireRate:
                manager.UpgradeBuyFireRate();
                break;
            default:
                break;
        }
    }
}
