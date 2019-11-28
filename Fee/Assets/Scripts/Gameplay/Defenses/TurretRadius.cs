using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretRadius : MonoBehaviour
{
    public delegate void onTurretTrigger(GameObject target);
    public onTurretTrigger onTurretDetectEnemy;
    public onTurretTrigger onTurretLostEnemy;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "enemy")
        {
            if(onTurretDetectEnemy != null)
            {
                onTurretDetectEnemy(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "enemy")
        {
            if (onTurretLostEnemy != null)
            {
                onTurretLostEnemy(other.gameObject);
            }
        }
    }
}
