using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerRadius : MonoBehaviour
{
    public bool canSpawn;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "npc")
        {
            canSpawn = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "npc")
        {
            canSpawn = true;
        }
    }
}
