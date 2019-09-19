using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyRadius : MonoBehaviour
{
    public delegate void OnRadiusAction(GameObject target);
    public OnRadiusAction OnRadiusFindWaypoint;

    /*// Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }*/

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
