using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComicPages : MonoBehaviour
{
    [System.Serializable]
    public class ComicPage
    {
        public GameObject parent;
        public GameObject[] images;
    }

    public GameObject panel;
    public ComicPage[] comicImages;
    public int currentPage;
    public int currentImage;
    public bool automaticClose;
    private bool[] firstTimeReading;
    public GameObject[] pages;

    // Start is called before the first frame update
    void Start()
    {
        firstTimeReading = new bool[comicImages.Length];
        pages = new GameObject[comicImages.Length];

        for (int i = 0; i < pages.Length; i++)
        {
            pages[i] = comicImages[i].parent;
            firstTimeReading[i] = false;
            ChangeAllImagesState(i, false);
        }
        
        OpenTutorial();
    }

    public void OpenTutorial()
    {
        currentPage = 0;
        currentImage = 0;

        for (int i = 0; i < pages.Length; i++)
        {
            if(pages[i])
            {
                pages[i].SetActive(false);
            }
        }

        pages[currentPage].SetActive(true);
        comicImages[currentPage].images[currentImage].SetActive(true);

        if (panel)
        {
            panel.SetActive(true);
        }
    }

    public void CloseTutorial()
    {
        if (panel)
        {
            panel.SetActive(false);
            ChangeAllImagesState(currentPage, false);
        }
    }

    public void NextPage()
    {
        if (!firstTimeReading[currentPage])
        {
            currentImage++;
        }
        else
        {
            CheckNextPage();
        }

        if(currentImage >= comicImages[currentPage].images.Length)
        {
            firstTimeReading[currentPage] = true;
            CheckNextPage();
        }
        else
        {
            if(!firstTimeReading[currentPage])
            {
                comicImages[currentPage].images[currentImage].SetActive(true);
            }
        }
    }

    public void PreviousPage()
    {
        ChangeAllImagesState(currentPage, false);
        pages[currentPage].SetActive(false);

        currentPage--;

        if (currentPage < 0)
        {
            currentPage = 0;
        }

        ChangeAllImagesState(currentPage, true);

        pages[currentPage].SetActive(true);
    }

    private void ChangeAllImagesState(int page, bool state)
    {
        for (int i = 0; i < comicImages[page].images.Length; i++)
        {
            comicImages[page].images[i].SetActive(state);
            currentImage = i;
        }
    }

    private void CheckNextPage()
    {
        ChangeAllImagesState(currentPage, false);
        currentImage = 0;
        pages[currentPage].SetActive(false);
        currentPage++;

        if (currentPage >= pages.Length)
        {
            if (automaticClose)
            {
                CloseTutorial();
            }

            currentPage = pages.Length-1;
            pages[currentPage].SetActive(true);
        }

        if (pages[currentPage])
        {
            pages[currentPage].SetActive(true);

            if (!firstTimeReading[currentPage])
            {
                comicImages[currentPage].images[currentImage].SetActive(true);
            }
            else
            {
                ChangeAllImagesState(currentPage, true);
            }
        }
    }
}
