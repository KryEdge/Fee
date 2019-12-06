using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretSpawner : MonoBehaviourSingleton<TurretSpawner>
{
    public delegate void OnSpawnerAction();
    public OnSpawnerAction OnSpawnerSpawnTurret;
    public OnSpawnerAction OnSpawnerDeleteTurret;
    public static OnSpawnerAction OnSpawnerSwitchTool;

    [Header("General Settings")]
    public KeyCode activateKey;
    public LayerMask Mask;
    public LayerMask deleteTurretMask;
    public float fireRate;

    [Header("Sound Settings")]
    public GameObject swapSound;
    public GameObject turretUnavailableSound;

    [Header("Assign Components/GameObjects")]
    public Button turretButton;
    public GameObject turretTemplate;
    public Shader spawnShader;

    [Header("Checking Variables")]
    public List<GameObject> spawnedTurrets;
    public bool preview;
    public bool canDelete;

    private UITowersState towerUIState;
    private Turret turretProperties;
    private SkinnedMeshRenderer turretMaterial;
    private GameObject myEventSystem;
    private GameObject newTurretPreview;
    private MaterialPropertyBlock material;
    private bool canSpawn;

    // Start is called before the first frame update
    private void Start()
    {
        myEventSystem = GameObject.Find("EventSystem");

        newTurretPreview = Instantiate(turretTemplate, turretTemplate.transform.position, turretTemplate.transform.rotation);
        turretProperties = newTurretPreview.GetComponent<Turret>();
        turretMaterial = newTurretPreview.GetComponent<Turret>().attachedModel;
        turretProperties.isPreview = true;
        turretProperties.attachedParticles.SetActive(false);
        newTurretPreview.GetComponent<BoxCollider>().isTrigger = true;
        turretProperties.turretRadius.gameObject.GetComponent<BoxCollider>().enabled = false;
        turretProperties.attachedVisionRadius.SetActive(true);
        newTurretPreview.SetActive(false);

        turretTemplate.GetComponent<FauxGravityBody>().isBuilding = true;

        material = new MaterialPropertyBlock();
        canSpawn = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!UIPauseButton.isGamePaused)
        {
            if (Input.GetKeyDown(activateKey))
            {
                SwitchPreview();

                if(OnSpawnerSwitchTool != null)
                {
                    OnSpawnerSwitchTool();
                }

                AkSoundEngine.PostEvent("swap_tool", swapSound);
            }

            if (Input.GetMouseButtonDown(2))
            {
                if (canDelete)
                {
                    DeleteTurret();
                }
            }

            if (preview)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (turretProperties.canBePlaced && turretProperties.isInTurretZone)
                    {
                        if (canSpawn)
                        {
                            Spawn();
                        }
                        else
                        {
                            //turretUnavailableSound
                            AkSoundEngine.PostEvent("torre_no", turretUnavailableSound);
                        }
                    }
                    else if (!turretProperties.canBePlaced || !turretProperties.isInTurretZone)
                    {
                        AkSoundEngine.PostEvent("torre_no", turretUnavailableSound);
                        Debug.Log("CANT SPAWN");
                    }
                }

                PreviewTurret();
            }
        }
    }

    private void Spawn()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray.origin, ray.direction, out hit, 999, Mask))
        {
            if (hit.transform.gameObject.tag != "turret" && hit.transform.gameObject.tag != "radius")
            {
                if (spawnedTurrets.Count <= GameManager.Get().maxTurrets - 1)
                {
                    //newTurretPreview.SetActive(false);
                    newTurretPreview.GetComponent<Turret>().enteredZones.Clear();
                    newTurretPreview.GetComponent<Turret>().canBePlaced = false;

                    GameManager.Get().towersPlaced++;

                    GameObject newTurret = Instantiate(turretTemplate, hit.point + (hit.normal * -5), newTurretPreview.transform.rotation);
                    newTurret.SetActive(true);
                    spawnedTurrets.Add(newTurret);

                    if (OnSpawnerSpawnTurret != null)
                    {
                        OnSpawnerSpawnTurret();
                    }
                    //
                    Turret currentTurretProperties = newTurret.GetComponent<Turret>();
                    List<UITowersState> state = GameManager.Get().towersUI;

                    currentTurretProperties.zone = newTurretPreview.GetComponent<Turret>().zone;
                    currentTurretProperties.zone.isUsed = true;
                    currentTurretProperties.isSpawned = true;
                    currentTurretProperties.OnTurretDead = DeleteTurretTimer;
                    currentTurretProperties.fireRate = fireRate;
                    currentTurretProperties.attachedModel.material.shader = spawnShader;
                    currentTurretProperties.lifespanTimer = 0;
                    Debug.Log("messi mode");
                    currentTurretProperties.attachedParticles.SetActive(true);

                    for (int i = state.Count - 1; i >= 0; i--)
                    {
                        if (!state[i].isBeingUsed)
                        {
                            currentTurretProperties.stateUI = state[i];
                            state[i].assignedTurret = currentTurretProperties;
                            state[i].isBeingUsed = true;
                            state[i].doOnce = false;
                            i = -1;
                        }
                    }

                    //newTurretPreview.SetActive(true);
                }
                else
                {
                    AkSoundEngine.PostEvent("torre_no", turretUnavailableSound);
                    Debug.Log("CANT SPAWN, MAX TOWERS");
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

                GameObject turretToDelete = null;

                foreach (GameObject turret in spawnedTurrets)
                {
                    if (turret == hit.transform.gameObject)
                    {
                        turretToDelete = turret;
                    }
                }

                if (turretToDelete)
                {
                    spawnedTurrets.Remove(turretToDelete);
                    turretToDelete.GetComponent<Turret>().zone.isUsed = false;
                    

                    Turret prewiewTurretProperties = newTurretPreview.GetComponent<Turret>();

                    foreach (GameObject item in prewiewTurretProperties.enteredTurrets)
                    {
                        if (item == turretToDelete)
                        {
                            prewiewTurretProperties.enteredTurrets.Remove(turretToDelete);
                        }
                    }

                    Destroy(turretToDelete);

                    if (OnSpawnerDeleteTurret != null)
                    {
                        OnSpawnerDeleteTurret();
                    }

                    newTurretPreview.GetComponent<Turret>().enteredTurrets.Remove(turretToDelete);
                    newTurretPreview.GetComponent<Turret>().CheckIfCanBePlaced();
                    newTurretPreview.SetActive(false);
                    newTurretPreview.SetActive(true);
                }

            }

            Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green);
        }
        else
        {
            Debug.DrawRay(ray.origin, ray.direction * 999, Color.white);
        }
    }

    private void DeleteTurretTimer(GameObject turret)
    {
        spawnedTurrets.Remove(turret);

        Turret prewiewTurretProperties = newTurretPreview.GetComponent<Turret>();

        foreach (GameObject item in prewiewTurretProperties.enteredTurrets)
        {
            if(item == turret)
            {
                prewiewTurretProperties.enteredTurrets.Remove(turret);
            }
        }
        //turret.GetComponent<Turret>().zone.isUsed = false;

        if (OnSpawnerDeleteTurret != null)
        {
            OnSpawnerDeleteTurret();
        }

        //newTurretPreview.SetActive(false);
        //newTurretPreview.SetActive(true);
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

        if (turretProperties.canBePlaced && turretProperties.isInTurretZone && spawnedTurrets.Count < GameManager.Get().maxTurrets)
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
