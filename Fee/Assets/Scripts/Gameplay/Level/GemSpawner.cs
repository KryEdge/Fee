using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemSpawner : MonoBehaviour
{
    [Header("General Settings")]
    public int gemsRewardAmount;
    public FauxGravityAttractor planet;
    public GameObject template;
    public GameObject UIGems;
    public float spawnerTimeMin;
    public float spawnerTimeMax;
    public Transform[] locations;
    public LayerMask Mask;

    public bool[] takenLocations;
    private Gem gemProperties;
    public float spawnerTimer = 0;
    private float spawnTimeFinal;
    private bool canSpawn = false;

    // Start is called before the first frame update
    void Start()
    {
        gemProperties = template.GetComponent<Gem>();
        //gemProperties.body.attractor = planet;
        template.SetActive(false);

        //takenLocations = new bool[locations.Length];
        RollRandomTime();
    }

    // Update is called once per frame
    void Update()
    {
        if(!canSpawn)
        {
            spawnerTimer += Time.deltaTime;

            if (spawnerTimer >= spawnTimeFinal)
            {
                canSpawn = true;
                spawnerTimer = 0;
                RollRandomTime();
            }
        }
        else
        {
            Spawn();
        }

        CheckGemPointer();
    }

    private void RollRandomTime()
    {
        float randomValue = Random.Range(spawnerTimeMin, spawnerTimeMax);

        spawnTimeFinal = randomValue;
    }

    private void Spawn()
    {
        gemProperties.gemsAmount = gemsRewardAmount;
        gemProperties.UIGems = UIGems;
        gemProperties.spawner = this;

        for (int i = 0; i < takenLocations.Length; i++)
        {
            if(!takenLocations[i])
            {
                gemProperties.id = i;
                GameObject newGem = Instantiate(template) as GameObject;
                newGem.transform.position = locations[i].position;
                newGem.GetComponent<Gem>().planet = planet;
                newGem.SetActive(true);
                takenLocations[i] = true;

                i = takenLocations.Length;
            }
        }

        canSpawn = false;
    }

    public void DeleteGem(Gem gem)
    {
        for (int i = 0; i < takenLocations.Length; i++)
        {
            if (gem.id == i)
            {
                takenLocations[i] = false;
                Destroy(gem.gameObject);
            }
        }
    }

    public void CheckGemPointer()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray.origin, ray.direction, out hit, 999, Mask))
        {

            if (hit.transform.gameObject.tag == "gem")
            {
                hit.transform.gameObject.GetComponent<Gem>().CollectGem();
            }

            Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green);
        }
        else
        {
            Debug.DrawRay(ray.origin, ray.direction * 999, Color.white);
        }
    }
}
