using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyRadius : MonoBehaviour
{
    public delegate void OnRadiusAction(GameObject target);
    public OnRadiusAction OnRadiusFindWaypoint;
    public OnRadiusAction OnRadiusFindEnemy;

    public bool findEnemy;

    private void OnTriggerEnter(Collider other)
    {
        if(findEnemy)
        {
            if (other.gameObject.tag == "enemy")
            {
                if (OnRadiusFindEnemy != null)
                {
                    OnRadiusFindEnemy(other.gameObject);
                }
            }
        }
        else
        {
            if (other.gameObject.tag == "waypoint")
            {
                if (OnRadiusFindWaypoint != null)
                {
                    OnRadiusFindWaypoint(other.gameObject);
                }
            }
        }
    }
}
