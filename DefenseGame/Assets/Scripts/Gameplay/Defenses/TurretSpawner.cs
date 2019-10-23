using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretSpawner : MonoBehaviourSingleton<TurretSpawner>
{
    [Header("General Settings")]
    public KeyCode activateKey;
    public LayerMask Mask;
    public LayerMask deleteTurretMask;
    public float fireRate;

    [Header("Assign Components/GameObjects")]
    public Button turretButton;
    public GameObject turretTemplate;

    [Header("Checking Variables")]
    public List<GameObject> spawnedTurrets;
    public UITowersState towerUIState;
    public GameObject newTurretPreview;
    public bool preview;
    public bool canDelete;

    private Turret turretProperties;
    private MeshRenderer turretMaterial;
    private GameObject myEventSystem;
    private MaterialPropertyBlock material;
    private bool canSpawn;

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
        canSpawn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(activateKey))
        {
            SwitchPreview();
        }

        if (Input.GetMouseButtonDown(2))
        {
            if (canDelete)
            {
                DeleteTurret();
            }
        }        
        
        if(preview)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (turretProperties.canBePlaced && turretProperties.isInTurretZone)
                {
                    if(canSpawn)
                    {
                        Spawn();
                    }
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

                    Turret currentTurretProperties = newTurret.GetComponent<Turret>();
                    List<UITowersState> state = GameManager.Get().towersUI;

                    currentTurretProperties.OnTurretDead = DeleteTurretTimer;
                    currentTurretProperties.fireRate = fireRate;

                    for (int i = state.Count-1; i >= 0; i--)
                    {
                        if (!state[i].isBeingUsed)
                        {
                            currentTurretProperties.stateUI = state[i];
                            state[i].assignedTurret = currentTurretProperties;
                            state[i].isBeingUsed = true;
                            i = -1;
                        }
                    }
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

                    newTurretPreview.GetComponent<Turret>().enteredTurrets.Remove(turretToDelete);
                    newTurretPreview.GetComponent<Turret>().CheckIfCanBePlaced();
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

    private void DeleteTurretTimer(GameObject turret)
    {
        spawnedTurrets.Remove(turret);
        GameManager.Get().UpdateUI();
    }

    private void PreviewTurret()
    {
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
            material.SetColor("_BaseColor", Color.green);
            turretMaterial.SetPropertyBlock(material);
        }
        else
        {
            material.SetColor("_BaseColor", Color.red);
            turretMaterial.SetPropertyBlock(material);
        }
    }

    public void SwitchPreview()
    {
        canSpawn = !GameManager.Get().shoot.isActivated;

        if(!canSpawn)
        {
            GameManager.Get().SwitchMeteorActivation();
        }

        canSpawn = !GameManager.Get().shoot.isActivated;
        newTurretPreview.SetActive(!newTurretPreview.activeSelf);
        preview = !preview;
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);

        if (preview)
        {
            if(turretButton)
            {
                turretButton.image.color = Color.green;
            }
            
        }
        else
        {
            if(turretButton)
            {
                turretButton.image.color = Color.white;
            }
            
            newTurretPreview.GetComponent<Turret>().enteredZones.Clear();
            newTurretPreview.GetComponent<Turret>().enteredTurrets.Clear();
            newTurretPreview.GetComponent<Turret>().isInTurretZone = false;
            GameManager.Get().SwitchMeteorActivation();
        }
    }

    public void SwitchPreviewForced()
    {
        canSpawn = !canSpawn;
        newTurretPreview.SetActive(!newTurretPreview.activeSelf);
        preview = !preview;
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);

        if (preview)
        {
            turretButton.image.color = Color.green;
        }
        else
        {
            turretButton.image.color = Color.white;
            newTurretPreview.GetComponent<Turret>().enteredZones.Clear();
            newTurretPreview.GetComponent<Turret>().enteredTurrets.Clear();
            newTurretPreview.GetComponent<Turret>().isInTurretZone = false;
        }
    }

    public void StopAllOutlines()
    {
        foreach (GameObject turret in spawnedTurrets)
        {
            turret.GetComponent<Turret>().TurnOffOutline();
        }
    }
}
