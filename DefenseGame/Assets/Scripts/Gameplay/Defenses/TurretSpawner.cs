using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretSpawner : MonoBehaviourSingleton<TurretSpawner>
{
    public LayerMask Mask;
    public LayerMask deleteTurretMask;
    public GameObject turretTemplate;
    public bool preview;
    public GameObject newTurretPreview;
    public List<GameObject> spawnedTurrets;

    private Turret turretProperties;
    private MeshRenderer turretMaterial;
    private GameObject myEventSystem;
    private MaterialPropertyBlock material;

    // Start is called before the first frame update
    void Start()
    {
        myEventSystem = GameObject.Find("EventSystem");
        newTurretPreview = Instantiate(turretTemplate, turretTemplate.transform.position, turretTemplate.transform.rotation);
        turretProperties = newTurretPreview.GetComponent<Turret>();
        turretMaterial = newTurretPreview.transform.GetChild(1).GetComponent<MeshRenderer>();
        turretProperties.isPreview = true;
        newTurretPreview.GetComponent<BoxCollider>().isTrigger = true;
        turretProperties.turretRadius.gameObject.GetComponent<BoxCollider>().enabled = false;
        newTurretPreview.SetActive(false);

        turretTemplate.GetComponent<FauxGravityBody>().isBuilding = true;

        material = new MaterialPropertyBlock();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DeleteTurret();
        }        
        
        if(preview)
        {
            if (Input.GetMouseButtonDown(2))
            {
                if (turretProperties.canBePlaced && turretProperties.isInTurretZone)
                {
                    Spawn();
                }
            }

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
                if(spawnedTurrets.Count <= GameManager.Get().maxTurrets - 1)
                {
                    GameObject newTurret = Instantiate(turretTemplate, hit.point + (hit.normal * 1), newTurretPreview.transform.rotation);
                    newTurret.SetActive(true);
                    spawnedTurrets.Add(newTurret);
                    GameManager.Get().UpdateUI();
                }
                
            }
        }
    }

    private void DeleteTurret()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        
        if (Physics.Raycast(ray.origin, ray.direction, out hit, 999, deleteTurretMask))
        {
            
            if (hit.transform.gameObject.tag == "turret")
            {
                Debug.Log("encontre");

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
                    Debug.Log("borranding");
                    spawnedTurrets.Remove(turretToDelete);
                    Destroy(turretToDelete);
                    GameManager.Get().UpdateUI();
                }
                
            }

            Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green);
        }
        else
        {
            Debug.Log("buscando torreta");
            Debug.DrawRay(ray.origin, ray.direction * 999, Color.white);
        }
    }

    private void PreviewTurret()
    {
        //Debug.Log("DOOU");

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray.origin, ray.direction, out hit, 999, Mask))
        {

            if (hit.transform.gameObject.tag != "turret")
            {
                newTurretPreview.transform.position = hit.point + (hit.normal * -3.0f);
            }
        }
        else
        {
            newTurretPreview.transform.position = ray.origin * -3;
        }

        if (turretProperties.canBePlaced && turretProperties.isInTurretZone)
        {
            //turretMaterial.material.color = Color.green;
            material.SetColor("_BaseColor", Color.green);
            turretMaterial.SetPropertyBlock(material);
        }
        else
        {
            //turretMaterial.material.color = Color.red;
            material.SetColor("_BaseColor", Color.red);
            turretMaterial.SetPropertyBlock(material);
        }
    }

    public void SwitchPreview()
    {
        newTurretPreview.SetActive(!newTurretPreview.activeSelf);
        preview = !preview;
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
    }
}
