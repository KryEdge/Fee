using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyRadius : MonoBehaviour
{
    public delegate void OnRadiusAction(GameObject target);
    public OnRadiusAction OnRadiusFindWaypoint;

    private void OnTriggerEnter(Collider other)
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
