using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ally : MonoBehaviour
{

    private Rigidbody rig;
    private Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody>();
        direction = new Vector3(1, 0, 0);
    }

    /*// Update is called once per frame
    void Update()
    {
        
    }*/

    private void FixedUpdate()
    {
        rig.MovePosition(rig.position + transform.TransformDirection(direction) * 5 * Time.deltaTime); ;
    }

}
