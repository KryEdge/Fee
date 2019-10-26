using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
    public FauxGravityAttractor planet;
    public GameObject lightPrefab;
    public GameObject prefab;
    //public static int fairySpeed;
    public static int prefabAmount;
    public static List<GameObject> fairies;
    public Transform[] positions;

    public int groupSize = 7;
    public static Vector3 goalPosition = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        fairies = new List<GameObject>();
        prefabAmount = GameManager.Get().maxFairies;

        for (int i = 0; i < prefabAmount; i++)
        {
            fairies.Add(Instantiate(prefab, prefab.transform.position, Quaternion.identity));
            fairies[i].GetComponent<FauxGravityBody>().attractor = planet;
            fairies[i].GetComponent<Fairy>().ChangeSpeed(GameManager.Get().fairySpeed);
            fairies[i].transform.position = positions[i].position;
            fairies[i].SetActive(true);
        }

        
    }

    private void Update()
    {
        if (fairies.Count > 0)
        {
            lightPrefab.transform.position = fairies[0].transform.position + fairies[0].transform.up * 3;
            lightPrefab.transform.rotation = fairies[0].transform.rotation;
        }
        else
        {
            lightPrefab.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        //prefabAmount = null;
        fairies = null;

    }

    public void KillAllFairies()
    {
        foreach (GameObject item in fairies)
        {
            Destroy(item);
        }

        fairies.Clear();
    }
}
