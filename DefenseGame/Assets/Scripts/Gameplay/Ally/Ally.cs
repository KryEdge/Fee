using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ally : MonoBehaviour
{
    private Rigidbody rig;
    public Vector3 direction;
    public float minTime;
    public float maxTime;
    public float randomTime;

    private float timer;
    private float slerpTimer;
    private float slerpMaxTime;
    private Vector3 oldDirection;
    private Vector3 newDirection;

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody>();
        randomDirection();
        newRandomTime();
       // direction = new Vector3(-1, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        slerpTimer += Time.deltaTime;

        if(timer >= randomTime)
        {
            randomDirection();
            newRandomTime();
            timer = 0;
        }

        if(slerpTimer <= 1)
        {
            newDirection = Vector3.Slerp(oldDirection, direction, slerpTimer);
        }
        
    }

    private void FixedUpdate()
    {
        rig.MovePosition(rig.position + transform.TransformDirection(newDirection) * 5 * Time.deltaTime); ;
    }

    private void randomDirection()
    {
        oldDirection = direction;
        int randomNumber = Random.Range(0, 4);       

        switch (randomNumber)
        {
            case 0:
                direction = new Vector3(-4, 0, 0);
                break;
            case 1:
                direction = new Vector3(4, 0, 0);
                break;
            case 2:
                direction = new Vector3(0, 0, 4);
                break;
            case 3:
                direction = new Vector3(0, 0, -4);
                break;
            default:
                break;
        }

        if(oldDirection != direction)
        {
            slerpTimer = 0;
        }
    }

    private void newRandomTime()
    {
        randomTime = Random.Range(minTime, maxTime);
    }
}
