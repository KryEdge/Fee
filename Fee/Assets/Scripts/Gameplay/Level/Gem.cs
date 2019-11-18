using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    [Header("Assign Components")]
    public GameObject model;

    [Header("General Settings")]
    public float distance;
    public float rotationSpeed;

    private Vector3 newPosition;
    private Vector3 oldPosition;
    private float final;
    private Rigidbody rig;

    // Start is called before the first frame update
    void Start()
    {
        oldPosition = model.transform.position - model.transform.up * distance;
        newPosition = model.transform.position + model.transform.up * distance;
        rig = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        oldPosition = transform.position - model.transform.up * distance;
        newPosition = transform.position + model.transform.up * distance;
        final = Hermite(Mathf.PingPong(Time.time * 1.5f, 1.0f)) * 1f;

        model.transform.position = Vector3.Slerp(oldPosition, newPosition, final);
        model.transform.Rotate(new Vector3(0, Time.deltaTime * rotationSpeed, 0));
    }

    private float Hermite(float t)
    {
        return -t * t * t * 2f + t * t * 3f;
    }
}
