using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
    public FauxGravityAttractor planet;
    public GameObject prefab;
    public static int prefabAmount = 3;
    //public static GameObject[] fairies = new GameObject[prefabAmount];
    public static List<GameObject> fairies = new List<GameObject>();

    public int groupSize = 3;
    public static Vector3 goalPosition = Vector3.zero;


    //private GameObject[] fairies;
    // Start is called before the first frame update
    void Start()
    {
        //fairies = GameManager.Get().fairies;
        //fairies = new GameObject[prefabAmount];

        for (int i = 0; i < prefabAmount; i++)
        {
            Vector3 pos = new Vector3(  transform.position.x + Random.Range(-groupSize, groupSize+1),
                                        transform.position.y,
                                        transform.position.z + Random.Range(-groupSize, groupSize + 1)
                                     );
            //fairies[i] = Instantiate(prefab, pos, Quaternion.identity);
            fairies.Add(Instantiate(prefab, pos, Quaternion.identity));

            

            
            fairies[i].GetComponent<FauxGravityBody>().attractor = planet;
            fairies[i].SetActive(true);
        }
    }

    /*// Update is called once per frame
    void Update()
    {

    }*/
}
