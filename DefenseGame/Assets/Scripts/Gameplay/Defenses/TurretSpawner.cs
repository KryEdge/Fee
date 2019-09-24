using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretSpawner : MonoBehaviour
{
    public LayerMask Mask;
    public GameObject turretTemplate;
    public bool preview;
    public GameObject newTurretPreview;

    private Turret turretProperties;
    private MeshRenderer turretMaterial;

    // Start is called before the first frame update
    void Start()
    {
        newTurretPreview = Instantiate(turretTemplate, turretTemplate.transform.position, turretTemplate.transform.rotation);
        turretProperties = newTurretPreview.GetComponent<Turret>();
        turretMaterial = newTurretPreview.GetComponent<MeshRenderer>();
        turretProperties.isPreview = true;
        //newTurretPreview.GetComponent<BoxCollider>().enabled = false;
        newTurretPreview.GetComponent<BoxCollider>().isTrigger = true;
        //newTurretPreview.GetComponent<Rigidbody>().enabled = false;
        //newTurretPreview.layer = 2;
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
                newTurret.GetComponent<BoxCollider>().enabled = true;
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

        if (turretProperties.canBePlaced)
        {
            turretMaterial.material.color = Color.green;
        }
        else
        {
            turretMaterial.material.color = Color.red;
        }
    }
}
