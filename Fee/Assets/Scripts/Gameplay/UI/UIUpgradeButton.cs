using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUpgradeButton : MonoBehaviour
{
    public enum UpgradeType
    {
        maxFairies,
        meteorCooldown,
        towersFireRate,
        fairySpeed,
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
            case UpgradeType.meteorCooldown:
                manager.UpgradeBuyMeteorCooldown();
                break;
            case UpgradeType.towersFireRate:
                manager.UpgradeBuyFireRate();
                break;
            case UpgradeType.fairySpeed:
                manager.UpgradeBuyFairySpeed();
                break;
            default:
                break;
        }
    }
}
