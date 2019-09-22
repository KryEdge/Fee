using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRadius : MonoBehaviour
{
    public delegate void OnRadiusAction(GameObject target);
    public OnRadiusAction OnRadiusFindWaypoint;
    public OnRadiusAction OnRadiusFindAlly;
    public OnRadiusAction OnRadiusLostAlly;

    public bool exitRadius;

    private void OnTriggerEnter(Collider other)
    {
        if(!exitRadius)
        {
            if (other.gameObject.tag == "waypoint")
            {
                if (OnRadiusFindWaypoint != null)
                {
                    OnRadiusFindWaypoint(other.gameObject);
                }
            }

            if (other.gameObject.tag == "npc")
            {
                if (OnRadiusFindAlly != null)
                {
                    OnRadiusFindAlly(other.gameObject);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (exitRadius)
        {
            if (other.gameObject.tag == "npc")
            {
                if (OnRadiusLostAlly != null)
                {
                    OnRadiusLostAlly(other.gameObject);
                }
            }
        }
        
    }
}
