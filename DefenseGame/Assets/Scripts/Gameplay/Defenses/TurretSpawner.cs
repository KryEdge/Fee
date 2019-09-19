using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretSpawner : MonoBehaviour
{
    public LayerMask Mask;
    public GameObject turretTemplate;
    public bool preview;
    public GameObject newTurretPreview;

    // Start is called before the first frame update
    void Start()
    {
        newTurretPreview = Instantiate(turretTemplate, turretTemplate.transform.position, turretTemplate.transform.rotation);
        newTurretPreview.GetComponent<Turret>().isPreview = true;
        newTurretPreview.SetActive(false);

        turretTemplate.GetComponent<FauxGravityBody>().isBuilding = true;
        //newTurretPreview.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            Spawn();
        }

        if(Input.GetKeyDown(KeyCode.Alpha0))
        {
            newTurretPreview.SetActive(!newTurretPreview.activeSelf);
            preview = !preview;
        }
        
        if(preview)
        {
            PreviewTurret();
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
                GameObject newTurret = Instantiate(turretTemplate, newTurretPreview.transform.position, newTurretPreview.transform.rotation);
                //newTurret.transform.position = newTurret.transform.position ;
                newTurret.SetActive(true);
                //shootOnce = true;
            }
        }
    }

    private void PreviewTurret()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray.origin, ray.direction, out hit, 999, Mask))
        {
            if (hit.transform.gameObject.tag != "turret")
            {
                newTurretPreview.transform.position = hit.point + (hit.normal * 5);
            }
        }
    }
}
