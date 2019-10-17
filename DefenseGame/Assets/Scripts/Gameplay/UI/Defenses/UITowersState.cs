using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITowersState : MonoBehaviour
{
    public bool isBeingUsed;

    public Turret assignedTurret;
    private Image icon;
    // Start is called before the first frame update
    void Start()
    {
        icon = GetComponent<Image>();
        GameManager.Get().towersUI.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
        if(assignedTurret)
        {
            float fill = ((assignedTurret.lifespanTimer * 100) / assignedTurret.lifespan) * 0.01f;

            icon.fillAmount = fill;

            if (assignedTurret.lifespanTimer >= assignedTurret.lifespan)
            {
                icon.fillAmount = 1;
            }
        }
    }
}
