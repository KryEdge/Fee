using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPages : MonoBehaviour
{
    public GameObject[] pages;
    public int currentPage;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(false);
        }

        pages[currentPage].SetActive(true);
    }

    /*// Update is called once per frame
    void Update()
    {
        
    }*/

    public void NextPage()
    {
        pages[currentPage].SetActive(false);

        currentPage++;

        if(currentPage >= pages.Length)
        {
            currentPage = 0;
        }

        pages[currentPage].SetActive(true);
    }

    public void PreviousPage()
    {
        pages[currentPage].SetActive(false);

        currentPage--;

        if (currentPage < 0)
        {
            currentPage = pages.Length-1;
        }

        pages[currentPage].SetActive(true);
    }
}
