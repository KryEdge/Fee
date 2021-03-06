﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
    [Header("General Settings")]
    public Transform[] positions;
    public int groupSize = 7;

    [Header("Asssign Components")]
    public GameObject list;
    public FauxGravityAttractor planet;
    public GameObject lightPrefab;
    public GameObject prefab;

    [Header("Checking Variables")]
    public static int prefabAmount;
    public static List<GameObject> fairies;
    public static List<Fairy> fairiesProperties;
    public static Vector3 goalPosition = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        fairies = new List<GameObject>();
        fairiesProperties = new List<Fairy>();
        prefabAmount = GameManager.Get().maxFairies;

        for (int i = 0; i < prefabAmount; i++)
        {
            fairies.Add(Instantiate(prefab, prefab.transform.position, Quaternion.identity));
            fairies[i].GetComponent<FauxGravityBody>().attractor = planet;
            fairies[i].GetComponent<Fairy>().ChangeSpeed(GameManager.Get().fairySpeed);
            fairies[i].transform.position = positions[i].position;
            fairies[i].SetActive(true);
            fairies[i].transform.SetParent(list.transform);
            fairiesProperties.Add(fairies[i].GetComponent<Fairy>());
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
