using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComicPages : MonoBehaviour
{
    [System.Serializable]
    public class ComicPage
    {
        public GameObject parent;
        public GameObject[] images;
    }

    [Header("General Settings")]
    public GameObject panel;
    public bool automaticClose;
    public ComicPage[] comicImages;
    public GameObject[] pages;

    [Header("Automatic Pass")]
    public KeyCode passKey;
    public float alphaSpeed;
    public float alphaTime;

    [Header("Text Settings")]
    public string nextPageString;
    public string completePageString;
    public Text indicationText;

    [Header("Checking Variables")]
    public int currentPage;
    public int currentImage;
    public float alpha;
    private bool[] firstTimeReading;
    private bool hasReachedAlpha;
    private Image currentPageImage;

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

    private void Update()
    {
        if(Input.GetKeyDown(passKey))
        {
            CheckNextPage();
        }

        if(!hasReachedAlpha)
        {
            alpha += Time.deltaTime * alphaSpeed;

            Color fade = currentPageImage.color;
            fade.a = alpha;

            currentPageImage.color = fade;

            if (alpha >= 1)
            {
                hasReachedAlpha = true;
                alpha = 1;
                Color finalFade = currentPageImage.color;
                finalFade.a = alpha;

                currentPageImage.color = finalFade;
                NextPageAutomatic();
            }
        }
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

        hasReachedAlpha = false;
        currentPageImage = comicImages[currentPage].images[currentImage].GetComponent<Image>();
        alpha = 0;
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
        CheckNextPage();
    }

    public void NextPageAutomatic()
    {
        if (!firstTimeReading[currentPage])
        {
            currentImage++;
        }
        else
        {
            CheckNextPage();
        }

        if (currentImage >= comicImages[currentPage].images.Length)
        {
            hasReachedAlpha = true;
            alpha = 1;
            if (currentPage < pages.Length-1)
            {
                indicationText.text = nextPageString;
            }
            else
            {
                indicationText.text = "";
            }
        }
        else
        {
            if (!firstTimeReading[currentPage])
            {
                comicImages[currentPage].images[currentImage].SetActive(true);
                currentPageImage = comicImages[currentPage].images[currentImage].GetComponent<Image>();
                Color finalFade = currentPageImage.color;
                finalFade.a = 0;
                currentPageImage.color = finalFade;
            }

            hasReachedAlpha = false;
            alpha = 0;
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

        hasReachedAlpha = true;
        alpha = 1;
        indicationText.text = nextPageString;
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

        if(currentImage < comicImages[currentPage].images.Length-1)
        {
            CompletePage();
            if (currentPage >= pages.Length-1)
            {
                indicationText.text = "";
            }
            else
            {
                indicationText.text = nextPageString;
            }
        }
        else if (currentImage >= comicImages[currentPage].images.Length-1)
        {
            
            ChangeAllImagesState(currentPage, false);
            currentImage = 0;
            pages[currentPage].SetActive(false);
            firstTimeReading[currentPage] = true;
            currentPage++;

            if (currentPage >= pages.Length)
            {
                indicationText.text = "";
                if (automaticClose)
                {
                    CloseTutorial();
                }

                currentPage = pages.Length - 1;
                pages[currentPage].SetActive(true);
                ChangeAllImagesState(currentPage, true);
                hasReachedAlpha = true;
                alpha = 1;
            }
            else if (pages[currentPage])
            {
                indicationText.text = completePageString;
                pages[currentPage].SetActive(true);

                if (!firstTimeReading[currentPage])
                {
                    comicImages[currentPage].images[currentImage].SetActive(true);
                    currentPageImage = comicImages[currentPage].images[currentImage].GetComponent<Image>();
                    Color finalFade = currentPageImage.color;
                    finalFade.a = 0;
                    currentPageImage.color = finalFade;
                    hasReachedAlpha = false;
                    alpha = 0;
                }
                else
                {
                    ChangeAllImagesState(currentPage, true);
                }
            }
        }
    }

    private void CompletePage()
    {
        ChangeAllImagesState(currentPage, true);
        hasReachedAlpha = true;
        alpha = 1;
        Color finalFade = currentPageImage.color;
        finalFade.a = alpha;

        currentPageImage.color = finalFade;
        firstTimeReading[currentPage] = true;
    }
}
