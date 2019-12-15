using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FauxGravityBody : MonoBehaviour
{
    public FauxGravityAttractor attractor;
    public bool useAlternativeGravity;
    public bool isBuilding;
    public float newGravity;
    private Rigidbody rig;
    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody>();
        if(!isBuilding)
        {
            rig.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }
        rig.useGravity = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.timeScale > 0)
        {
            if (useAlternativeGravity)
            {
                attractor.Attract(transform, rig, newGravity);
            }
            else
            {
                attractor.Attract(transform, rig);
            }
        }
    }
}
