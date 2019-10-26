using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoaderManager : MonoBehaviourSingleton<LoaderManager>
{
    public float loadingProgress;
    public bool fakeLoad;
    public float timeLoading;
    public float minTimeToLoad = 2;
    public void LoadScene(string sceneName)
    {
        StartCoroutine(AsynchronousLoad(sceneName));
    }

    IEnumerator AsynchronousLoad(string scene)
    {
        loadingProgress = 0;
        timeLoading = 0;
        yield return null;

        AsyncOperation ao = SceneManager.LoadSceneAsync(scene);
        ao.allowSceneActivation = false;

        while (!ao.isDone)
        {
            Debug.Log("LOADING " + timeLoading);

            timeLoading += Time.deltaTime*1.5f;
            loadingProgress = ao.progress + 0.1f;
            loadingProgress = loadingProgress * timeLoading / minTimeToLoad;

            Debug.Log("LOADING 2 " + timeLoading);

            // Loading completed
            if (loadingProgress >= 1)
            {
                ao.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
