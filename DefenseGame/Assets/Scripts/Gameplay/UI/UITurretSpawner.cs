using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITurretSpawner : MonoBehaviour
{
    private TurretSpawner spawner;

    // Start is called before the first frame update
    void Start()
    {
        spawner = TurretSpawner.Get();
    }

    public void SwitchSpawner()
    {
        spawner.SwitchPreview();
    }
}
