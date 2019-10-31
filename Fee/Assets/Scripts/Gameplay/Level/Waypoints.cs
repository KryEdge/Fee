using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    public GameObject[] nextWaypoints;

    private void Start()
    {
#if UNITY_STANDALONE && !UNITY_EDITOR
        GetComponent<MeshRenderer>().enabled = false;
#endif
    }
}
