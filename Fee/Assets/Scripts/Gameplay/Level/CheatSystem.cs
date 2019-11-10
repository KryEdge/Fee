using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatSystem : MonoBehaviour
{
    public delegate void OnCheatAction();
    public OnCheatAction OnCheatGivePoints;

    [Header("General Settings")]
    public GameObject cheatsScreen;
    public static bool isActivated;
    public bool isTimeNormal;

    [Header("Assign Components")]
    public FlockManager flockManager;
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

        if(OnCheatGivePoints != null)
        {
            OnCheatGivePoints();
        }

        //gameManager.scoreUI.UpdateText();
    }

    public void SwitchTowerDeletion()
    {
        TurretSpawner.Get().canDelete = !TurretSpawner.Get().canDelete;
    }

    public void KillAllEnemies()
    {
        gameManager.KillAllEnemies();
    }

    public void KillAllFairies()
    {
        flockManager.KillAllFairies();
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
