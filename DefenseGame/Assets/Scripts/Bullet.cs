using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject from;
    public GameObject target;
    public bool isFired = false;
    public Vector3 dirFrom;
    public float lifespan;

    private Vector3 dir;
    private GameObject objectAffected;
    private Rigidbody rig;
    // private Turret playerTurret;

    // Start is called before the first frame update
    private void Start()
    {
        rig = GetComponent<Rigidbody>();
        dirFrom = from.transform.position;
        transform.position = dirFrom;
        //playerTurret = player.GetComponent<Turret>();
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        //lifespan += Time.deltaTime;
        Quaternion q01 = Quaternion.identity;
        ////
        if (target)
        {
            //dir = transform.position - target.transform.position;
            //q01.SetLookRotation(target.transform.position - transform.position, transform.up);

            Vector3 direction = (target.transform.position - transform.position).normalized;
            rig.MovePosition(transform.position + direction * 120 * Time.deltaTime);
            rig.MoveRotation(target.transform.rotation);
            //transform.rotation = Quaternion.LookRotation(target.transform.position);
        }
        else
        {
            Destroy(gameObject);
        }

        //transform.position = transform.position - dir.normalized * 80.5f * Time.deltaTime;
        //transform.rotation = q01;

        if (isFired)
        {
            if (lifespan > 5)
            {
                lifespan = 0;
                isFired = false;
                //playerTurret.isFiring = false;
                Destroy(gameObject);
            }
        }
    }

    /*public void DestroyMissile()
    {
        isFired = false;
        playerTurret.isFiring = false;
        Destroy(gameObject);
    }*/
}
