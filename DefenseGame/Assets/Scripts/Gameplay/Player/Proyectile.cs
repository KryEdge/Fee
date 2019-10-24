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
    }

    // Update is called once per frame
    void Update()
    {
        if(target)
        {
            Vector3 direction = (target.transform.position - transform.position).normalized;
            rig.MovePosition(rig.position + direction * speed * Time.deltaTime);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
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

        if (collision.gameObject.tag == "dead")
        {
            Debug.Log("Proyectile hit enemy");
            Destroy(gameObject);
        }

        Debug.Log("Bullet collision with : " + collision.gameObject.tag);
    }
}
