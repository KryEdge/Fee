using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIChangeScene : MonoBehaviour
{
    public string sceneName;

    public void ForcedChangeScene()
    {
        if (SceneManager.GetActiveScene().name == "Gameplay")
        {
            Destroy(GameManager.Get().gameObject);
            Destroy(WaveSystem.Get().gameObject);
            Destroy(TurretSpawner.Get().gameObject);
            Destroy(Highscore.Get().gameObject);
            Destroy(MilestoneManager.Get().gameObject);

            UpgradeSystem upgrades = UpgradeSystem.Get();

            if (upgrades)
            {
                upgrades.CleanList();
            }
        }

        if (sceneName == "Gameplay")
        {
            UpgradeSystem upgrades = UpgradeSystem.Get();

            if (upgrades)
            {
                upgrades.CleanList();
            }

            LoaderManager.Get().LoadScene(sceneName);
                UILoadingScreen.Get().SetVisible(true);
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }

        Time.timeScale = 1;
    }

    public void ChangeScene()
    {
        if(SceneManager.GetActiveScene().name == "Gameplay")
        {
            Destroy(GameManager.Get().gameObject);
            Destroy(WaveSystem.Get().gameObject);
            Destroy(TurretSpawner.Get().gameObject);
            Destroy(Highscore.Get().gameObject);
            Destroy(MilestoneManager.Get().gameObject);

            UpgradeSystem upgrades = UpgradeSystem.Get();

            if (upgrades)
            {
                upgrades.CleanList();
            }
        }

        if (sceneName == "Upgrade Screen")
        {
            UpgradeSystem upgrades = UpgradeSystem.Get();

            if(upgrades)
            {
                upgrades.CleanList();
            }

            string goToTutorial = PlayerPrefs.GetString("isFirstTimePlaying", "yes");

            if (goToTutorial == "yes")
            {
                SceneManager.LoadScene("Tutorial");
            }
            else if (goToTutorial == "no")
            {
                SceneManager.LoadScene(sceneName);
            }
        }
        else if (sceneName == "Gameplay")
        {
            UpgradeSystem upgrades = UpgradeSystem.Get();

            if (upgrades)
            {
                upgrades.CleanList();
            }

            string goToTutorial = PlayerPrefs.GetString("isFirstTimePlaying", "yes");

            if (goToTutorial == "yes")
            {
                SceneManager.LoadScene("Tutorial");
            }
            else if (goToTutorial == "no")
            {
                LoaderManager.Get().LoadScene(sceneName);
                UILoadingScreen.Get().SetVisible(true);
            }
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }

        Time.timeScale = 1;
    }

    public void RepeatScene()
    {
        if (SceneManager.GetActiveScene().name == "Gameplay")
        {
            Destroy(GameManager.Get().gameObject);
            Destroy(WaveSystem.Get().gameObject);
            Destroy(TurretSpawner.Get().gameObject);
            Destroy(Highscore.Get().gameObject);
            Destroy(MilestoneManager.Get().gameObject);
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;

        UpgradeSystem upgrades = UpgradeSystem.Get();

        if (upgrades)
        {
            upgrades.CleanList();
        }
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}
