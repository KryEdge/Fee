using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
    public FauxGravityAttractor planet;
    public GameObject prefab;
    public static int prefabAmount = 6;
    public static GameObject[] allPrefabs = new GameObject[prefabAmount];

    public int groupSize = 3;
    public static Vector3 goalPosition = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < prefabAmount; i++)
        {
            Vector3 pos = new Vector3(  transform.position.x + Random.Range(-groupSize, groupSize+1),
                                        transform.position.y,
                                        transform.position.z + Random.Range(-groupSize, groupSize + 1)
                                     );

            allPrefabs[i] = Instantiate(prefab, pos, Quaternion.identity);
            allPrefabs[i].GetComponent<FauxGravityBody>().attractor = planet;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
