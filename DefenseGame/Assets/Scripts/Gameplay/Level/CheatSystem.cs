using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatSystem : MonoBehaviour
{
    [Header("General Settings")]
    public GameObject cheatsScreen;
    public bool isActivated;
    public bool isTimeNormal;

    [Header("Assign Components")]
    public Shoot meteor;
    private GameManager gameManager;

    // Start is called before the first frame update
    private void Start()
    {
        cheatsScreen.SetActive(false);
        gameManager = GameManager.Get();
    }

    public void SwitchScreen()
    {
        isActivated = !isActivated;
        cheatsScreen.SetActive(isActivated);
    }

    public void ReloadMeteor()
    {
        meteor.Reload();
    }

    public void SwitchFairiesInmunity()
    {
        Fairy.SwitchInvincibility();
    }

    public void StopWave()
    {
        gameManager.StopWave();
    }

    public void GivePoints()
    {
        gameManager.score += 100;
        gameManager.scoreUI.UpdateText();
    }

    public void SwitchTowerDeletion()
    {
        TurretSpawner.Get().canDelete = !TurretSpawner.Get().canDelete;
    }

    public void KillAllEnemies()
    {
        gameManager.KillAllEnemies();
    }

    public void StopTime()
    {
        isTimeNormal = !isTimeNormal;

        if(isTimeNormal)
        {
            Time.timeScale = 1;
        }
        else
        {
            Time.timeScale = 0;
        }
    }
}
