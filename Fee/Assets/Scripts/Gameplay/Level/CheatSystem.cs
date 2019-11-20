using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CheatSystem : MonoBehaviour
{
    public delegate void OnCheatAction();
    public OnCheatAction OnCheatGivePoints;

    [Header("General Settings")]
    public PostProcessVolume post;
    public GameObject cheatsScreen;
    public static bool isActivated;
    public static bool isTimeNormal = true;

    [Header("Assign Components")]
    public FlockManager flockManager;
    public Shoot meteor;
    private GameManager gameManager;
    private AutoExposure ae;
    public bool canStopTime;
    public float stopTimeTimer;

    // Start is called before the first frame update
    private void Start()
    {
        cheatsScreen.SetActive(false);
        gameManager = GameManager.Get();
        post.profile.TryGetSettings(out ae);

        
        ae.active = true;
        ae.minLuminance.value = 0;
        ae.keyValue.value = 1;  //compensation 1.22f;
    }

    private void Update()
    {
        if(canStopTime)
        {
            stopTimeTimer += Time.deltaTime;

            if(stopTimeTimer >= 3)
            {
                stopTimeTimer = 0;
                canStopTime = false;
                SwitchTime();
            }
        }
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

        if (isTimeNormal)
        {
            SwitchTime();
        }
        else
        {
            canStopTime = true;
        }
    }

    private void SwitchTime()
    {
        if (isTimeNormal)
        {
            ae.minLuminance.value = 0;
            ae.keyValue.value = 1.0f;
            Time.timeScale = 1;
        }
        else
        {
            ae.minLuminance.value = -9;
            ae.keyValue.value = 1.22f;
            Time.timeScale = 0;
        }
    }
}
