using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public enum Mode
    {
        bullet,
        meteor,
        allModes
    }

    public Mode mode;
    public GameObject from;
    public bool isFired = false;
    public bool hasCrashed;
    public Vector3 target;
    public Vector3 dirFrom;
    public float meteorSpeed;
    public float lifespanLimit;
    public float lifespan;
    public float explosionLimit;
    public float explosionTimer;

    private Vector3 dir;
    private GameObject objectAffected;
    private Rigidbody rig;
    private SphereCollider colliderMeteor;
    private MeshRenderer mesh;
    private FauxGravityBody fauxBody;
    private float distance;

    // Start is called before the first frame update
    private void Start()
    {
        rig = GetComponent<Rigidbody>();
        mesh = GetComponent<MeshRenderer>();
        fauxBody = GetComponent<FauxGravityBody>();
        colliderMeteor = GetComponent<SphereCollider>();

        dirFrom = from.transform.position;
        transform.position = dirFrom;
    }

    private void Update()
    {
        if (isFired)
        {
            lifespan += Time.deltaTime;
            distance = Vector3.Distance(target, transform.position);
            //Debug.Log(distance);

            if (lifespan > lifespanLimit)
            {
                DestroyBullet();
            }

            if (distance <= 1)
            {
                if (mode == Mode.bullet)
                {
                    DestroyBullet();
                }
            }
        }

        if (hasCrashed)
        {
            explosionTimer += Time.deltaTime;

            if (explosionTimer >= explosionLimit)
            {
                Destroy(gameObject);
            }
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!hasCrashed)
        {
            Vector3 direction = (target - transform.position).normalized;
            rig.MovePosition(transform.position + direction * meteorSpeed * Time.deltaTime);
            transform.LookAt(target);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (mode == Mode.meteor)
        {
            if (collision.gameObject.tag == "planet")
            {
                rig.constraints = RigidbodyConstraints.FreezeAll;
                mesh.enabled = false;
                fauxBody.enabled = false;
                colliderMeteor.enabled = false;

                transform.GetChild(0).gameObject.SetActive(true);
                hasCrashed = true;
                Debug.Log(collision.gameObject.tag);
            }
        }
    }

    private void DestroyBullet()
    {
        lifespan = 0;
        isFired = false;
        Destroy(gameObject);
    }
}
