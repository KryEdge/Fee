using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretSpawner : MonoBehaviour
{
    public LayerMask Mask;
    public GameObject turretTemplate;
    public bool preview;
    public int maxTurrets;
    public GameObject newTurretPreview;
    public List<GameObject> spawnedTurrets;


    private Turret turretProperties;
    private MeshRenderer turretMaterial;

    // Start is called before the first frame update
    void Start()
    {
        newTurretPreview = Instantiate(turretTemplate, turretTemplate.transform.position, turretTemplate.transform.rotation);
        turretProperties = newTurretPreview.GetComponent<Turret>();
        turretMaterial = newTurretPreview.GetComponent<MeshRenderer>();
        turretProperties.isPreview = true;
        //turretProperties.canBePlaced = true;
        newTurretPreview.GetComponent<BoxCollider>().isTrigger = true;
        //newTurretPreview.SetActive(true);
        turretProperties.turretRadius.gameObject.GetComponent<BoxCollider>().enabled = false;
        newTurretPreview.SetActive(false);

        turretTemplate.GetComponent<FauxGravityBody>().isBuilding = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            if (turretProperties.canBePlaced)
            {
                Spawn();
            } 
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
                if(spawnedTurrets.Count <= maxTurrets-1)
                {
                    GameObject newTurret = Instantiate(turretTemplate, hit.point + (hit.normal * 15), newTurretPreview.transform.rotation);
                    newTurret.SetActive(true);
                    spawnedTurrets.Add(newTurret);
                }
                
            }

            /*if (hit.transform.gameObject.tag == "turret")
            {
                GameObject turretToDelete = null;

                foreach (GameObject turret in spawnedTurrets)
                {
                    if(turret == hit.transform.gameObject)
                    {
                        turretToDelete = turret;
                    }
                }
                
                if(turretToDelete)
                {
                    spawnedTurrets.Remove(turretToDelete);
                }
                
            }*/
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
