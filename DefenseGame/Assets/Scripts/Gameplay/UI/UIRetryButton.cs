using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIRetryButton : MonoBehaviour
{
    public void RepeatScene()
    {
        Destroy(GameManager.Get().gameObject);
        Destroy(WaveSystem.Get().gameObject);
        Destroy(TurretSpawner.Get().gameObject);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }
}
