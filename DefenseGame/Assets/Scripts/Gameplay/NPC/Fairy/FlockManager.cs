using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
    public FauxGravityAttractor planet;
    public GameObject light;
    public GameObject prefab;
    public static int prefabAmount;
    public static List<GameObject> fairies;

    public int groupSize = 2;
    public static Vector3 goalPosition = Vector3.zero;

    // Start is called before the first frame update
    void Awake()
    {
        fairies = new List<GameObject>();
        prefabAmount = GameManager.Get().maxFairies;

        for (int i = 0; i < prefabAmount; i++)
        {
            Vector3 pos = new Vector3(  transform.position.x + Random.Range(-groupSize, groupSize+1),
                                        transform.position.y,
                                        transform.position.z + Random.Range(-groupSize, groupSize + 1)
                                     );
            fairies.Add(Instantiate(prefab, pos, Quaternion.identity));
            fairies[i].GetComponent<FauxGravityBody>().attractor = planet;
            fairies[i].SetActive(true);
        }

        
    }

    private void Update()
    {
        if (fairies.Count > 0)
        {
            light.transform.position = fairies[0].transform.position + fairies[0].transform.up * 3;
            light.transform.rotation = fairies[0].transform.rotation;
        }
        else
        {
            light.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        //prefabAmount = null;
        fairies = null;

    }
}
