using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FauxGravityAttractor : MonoBehaviour
{
    public float gravity = -10.0f;

    public void Attract(Transform body,Rigidbody bodyRig)
    {
        Vector3 gravityUp = (body.position - transform.position).normalized;
        Vector3 bodyUp = body.up;

        bodyRig.AddForce(gravityUp * gravity);

        Quaternion targetRotation = Quaternion.FromToRotation(bodyUp,gravityUp) * body.rotation;
        body.rotation = Quaternion.Slerp(body.rotation,targetRotation,50.0f * Time.deltaTime);
    }
}
