using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFakeEffect : MonoBehaviour
{
    public GameObject target;

    // Update is called once per frame
    private void Update()
    {
        transform.LookAt(target.transform);
    }
}
