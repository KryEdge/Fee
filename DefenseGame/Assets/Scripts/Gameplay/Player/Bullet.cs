using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject from;
    public Vector3 target;
    public bool isFired = false;
    public Vector3 dirFrom;
    public float lifespanLimit;
    public float lifespan;

    private Vector3 dir;
    private GameObject objectAffected;
    private Rigidbody rig;
    private float distance;

    // Start is called before the first frame update
    private void Start()
    {
        rig = GetComponent<Rigidbody>();
        dirFrom = from.transform.position;
        transform.position = dirFrom;
    }

    private void Update()
    {
        if (isFired)
        {
            lifespan += Time.deltaTime;
            distance = Vector3.Distance(target, transform.position);
            Debug.Log(distance);

            if (lifespan > lifespanLimit || distance <= 1)
            {
                lifespan = 0;
                isFired = false;
                Destroy(gameObject);
            }
        }

        
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        Vector3 direction = (target - transform.position).normalized;
        rig.MovePosition(transform.position + direction * 120 * Time.deltaTime);
        transform.LookAt(target);
    }

    /*public void DestroyMissile()
    {
        isFired = false;
        playerTurret.isFiring = false;
        Destroy(gameObject);
    }*/
}
