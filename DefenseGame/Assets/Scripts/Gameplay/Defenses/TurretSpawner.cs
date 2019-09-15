using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretSpawner : MonoBehaviour
{
    public LayerMask Mask;
    public GameObject turretTemplate;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            Spawn();
        }    
    }

    private void Spawn()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray.origin, ray.direction, out hit, 999, Mask))
        {
            if (hit.transform.gameObject.tag != "turret")
            {
                Debug.Log("Spawning");
                //bulletProperties.isFired = true;
                //bulletProperties.target = hit.point;
                GameObject newTurret = Instantiate(turretTemplate, hit.point + (turretTemplate.transform.up * 5), turretTemplate.transform.rotation);
                //newTurret.transform.position = newTurret.transform.position ;
                newTurret.SetActive(true);
                //shootOnce = true;
            }
        }
    }
}
