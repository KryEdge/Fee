using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    public delegate void OnGemAction();
    public static OnGemAction OnGemCollected;

    [Header("Assign Components")]
    public GameObject model;
    public GameObject UIGems;
    public GameObject particles1;
    public GameObject particles2;

    [Header("Sound Settings")]
    public GameObject gemCollectSound;

    [Header("General Settings")]
    public float lifespan;
    public int gemsAmount;
    public float distance;
    public float rotationSpeed;
    public float scaleSpeed;
    public float collectSpeed;

    [Header("Settings for Spawner")]
    public int id;
    public FauxGravityAttractor planet;

    public GemSpawner spawner;
    public FauxGravityBody body;
    private BoxCollider gemCollider;
    private Vector3 newPosition;
    private Vector3 oldPosition;
    private float final;
    private float lifespanTimer;
    private float disappearTimer;
    private Rigidbody rig;
    private bool isCollected;
    private bool isFullyCollected;

    // Start is called before the first frame update
    void Start()
    {
        oldPosition = model.transform.position - model.transform.up * distance;
        newPosition = model.transform.position + model.transform.up * distance;
        rig = GetComponent<Rigidbody>();
        body = GetComponent<FauxGravityBody>();
        gemCollider = GetComponent<BoxCollider>();
        body.attractor = planet;
    }

    // Update is called once per frame
    void Update()
    {
        if(isCollected)
        {
            Vector3 screenPoint = UIGems.transform.position + new Vector3(0, 0, 5);
            Vector3 gemsPosition = Camera.main.ScreenToWorldPoint(screenPoint);
            float gemsUIDistance = Vector3.Distance(transform.position, gemsPosition);

            if(gemsUIDistance <= 1.0f) // ajustar
            {
                isFullyCollected = true;
            }

            if(isFullyCollected)
            {
                Debug.Log("GIVE " + gemsAmount + " GEMS");
                GameManager.Get().upgradePointsCurrentMatch += gemsAmount;

                if(OnGemCollected != null)
                {
                    OnGemCollected();
                }

                if(spawner)
                {
                    spawner.DeleteGem(this);
                }
                else
                {
                    Destroy(gameObject);
                }

                GameManager.Get().gemsCollected += gemsAmount;

                AkSoundEngine.PostEvent("gema_recolectar", gemCollectSound);
                //Debug.Log("LAST METEOR");
            }
        }
        else
        {
            lifespanTimer += Time.deltaTime;

            if(lifespanTimer >= lifespan)
            {
                body.enabled = false;
                particles1.SetActive(false);
                particles2.SetActive(false);
                rig.isKinematic = true;
                transform.localScale = Vector3.Slerp(transform.localScale, new Vector3(-0.1f, -0.1f, -0.1f), Time.deltaTime * scaleSpeed);

                if(transform.localScale.x <= 0)
                {
                    gemCollider.enabled = false;
                    if (spawner)
                    {
                        spawner.DeleteGem(this);
                    }
                    else
                    {
                        Destroy(gameObject);
                    }
                }
            }

            oldPosition = transform.position - model.transform.up * distance;
            newPosition = transform.position + model.transform.up * distance;
            final = Hermite(Mathf.PingPong(Time.time * 1.5f, 1.0f)) * 1f;

            model.transform.position = Vector3.Slerp(oldPosition, newPosition, final);
            model.transform.Rotate(new Vector3(0, Time.deltaTime * rotationSpeed, 0));
        }
    }

    private void FixedUpdate()
    {
        if (isCollected)
        {
            Vector3 screenPoint = UIGems.transform.position + new Vector3(0, 0, 5);
            Vector3 gemsPosition = Camera.main.ScreenToWorldPoint(screenPoint);
            transform.position = Vector3.MoveTowards(transform.position, gemsPosition, Time.deltaTime * collectSpeed);

            transform.localScale = Vector3.MoveTowards(transform.localScale, new Vector3(0, 0, 0), Time.deltaTime * scaleSpeed);
        }

    }

    private float Hermite(float t)
    {
        return -t * t * t * 2f + t * t * 3f;
    }

    public void CollectGem()
    {
        body.enabled = false;
        particles1.SetActive(false);
        particles2.SetActive(false);
        rig.isKinematic = true;
        isCollected = true;
    }

    void OnMouseOver()
    {
        //If your mouse hovers over the GameObject with the script attached, output this message
        Debug.Log("Mouse is over GameObject.");
        CollectGem();
    }
}
