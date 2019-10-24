using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCRadius : MonoBehaviour
{
    public delegate void OnRadiusAction(GameObject target);
    public OnRadiusAction OnRadiusFindEnemy;
    public OnRadiusAction OnRadiusFindAlly;

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "enemy":
                if(OnRadiusFindEnemy != null)
                {
                    //Debug.Log("se murio este: " + other.gameObject.name);
                    OnRadiusFindEnemy(other.gameObject);
                }
                break;
            case "npc":
                if (OnRadiusFindAlly != null)
                {
                    OnRadiusFindAlly(other.gameObject);
                }
                break;
            default:
                break;
        }
    }
}
