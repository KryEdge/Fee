using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proyectile : MonoBehaviour
{
    public float speed;
    public GameObject target;
    public Vector3 startPosition;

    private Rigidbody rig;
    private GameManager gm;

    private TorqueLookRotation torque;
    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody>();
        gm = GameManager.Get();
        torque = GetComponent<TorqueLookRotation>();
        torque.target = target.transform;
        transform.position = startPosition;
        //destination = new Vector3(target.transform.position.x,transform.position.y, target.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        /*float distance = Vector3.Distance(gm.waypoints[currentTarget].transform.position, transform.position);

        if (distance <= 2.0f)
        {
            Debug.Log("eh messi");

            if (currentTarget == finalSpawnWaypoint)
            {
                currentTarget = entryWaypoint;
            }
            else
            {
                currentTarget++;
            }

            if (currentTarget >= gm.waypoints.Length)
            {
                currentTarget = 0;
            }

            
        }*/
        if(target)
        {
            Vector3 direction = (target.transform.position - transform.position).normalized;
            rig.MovePosition(rig.position + direction * speed * Time.deltaTime);
        }

    }

    /*private void OnMouseDown()
    {
        if (OnEnemyClicked != null)
        {
            OnEnemyClicked(gameObject);
        }
        //Destroy(gameObject);
    }*/

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Fui movido por : " + collision.gameObject.tag);

        /*if (collision.gameObject.tag == "bullet")
        {
            Debug.Log("Collision Test");
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }*/


        if (collision.gameObject.tag == "planet")
        {
            Debug.Log("Bullet didnt make it.");
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "enemy")
        {
            Debug.Log("Proyectile hit enemy");
            Destroy(gameObject);
        }
    }


    /*private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "explosion")
        {
            Debug.Log("Collision Test EXPLOSION");
            Destroy(gameObject);
        }
    }*/
}
