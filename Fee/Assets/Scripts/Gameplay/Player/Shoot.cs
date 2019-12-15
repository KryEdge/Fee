using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Shoot : MonoBehaviour
{
    public delegate void OnShootAction();
    public static OnShootAction OnShootMeteor;
    public static OnShootAction OnShootMeteorSecond;

    [Header("General Settings")]
    public KeyCode activateKey;
    public float fireRate;
    public float rechargeTime;
    public float meteorSpeed;
    public int maxMeteors;
    public LayerMask Mask;

    [Header("Sound Settings")]
    public GameObject lastMeteorSound;
    public GameObject shootMeteorSound;
    public GameObject unavailableSound;
    public GameObject cooldownSound;

    [Header("Assign Components")]
    public GameObject bulletTemplate;
    public GameObject explosionRadiusTemplate;

    [Header("UI Settings")]
    public Button meteorButton;
    public Text meteorCountText;
    public Color disableColor;
    public Color enableColor;
    private UIAnimation uiAnimation;

    [Header("Checking Variables")]
    public bool isActivated;

    private GameObject explosionRadius;
    private Bullet bulletProperties;
    private bool shootOnce;
    private bool canShoot;
    private bool isMouseOver;
    private int currentMeteors;
    private float fireRateTimer;
    private float rechargeTimer;
    private bool secondTime = false;
    // Start is called before the first frame update
    void Start()
    {
        TutorialEvents.OnEventMeteorClose += TurnSecondTimeON;

        bulletProperties = bulletTemplate.GetComponent<Bullet>();

        currentMeteors = maxMeteors;
        explosionRadius = Instantiate(explosionRadiusTemplate);

        uiAnimation = meteorButton.GetComponentInParent<UIAnimation>();

        UpdateText();
        SwitchActivation();
    }

    private void Update()
    {
        if(!UIPauseButton.isGamePaused)
        {
            if (currentMeteors < maxMeteors)
            {
                rechargeTimer += Time.deltaTime;

                float fill = ((rechargeTimer * 100) / rechargeTime) * 0.01f;

                meteorButton.image.fillAmount = fill;

                if (rechargeTimer >= rechargeTime)
                {
                    Reload();
                }
            }

            fireRateTimer += Time.deltaTime;

            if (fireRateTimer >= fireRate)
            {
                canShoot = true;
                fireRateTimer = 0;
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (canShoot)
                {
                    if (isActivated && !GameManager.Get().turretSpawner.preview && !isMouseOver)
                    {
                        if (ShootWeapon())
                        {
                            shootOnce = false;
                            canShoot = false;
                        }
                    }
                }
            }

            if (isActivated)
            {
                PreviewRadius();
            }
        }
    }

    private bool ShootWeapon()
    {
        bool couldShoot = false;

        if(currentMeteors > 0)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            

            if (Physics.Raycast(ray.origin, ray.direction, out hit, 999, Mask))
            {
                if (hit.transform.gameObject.tag != "explosion" && Time.timeScale != 0 && !EventSystem.current.IsPointerOverGameObject())
                {
                    if (!shootOnce)
                    {
                        bulletProperties.isFired = true;
                        bulletProperties.target = hit.point;
                        bulletProperties.meteorSpeed = meteorSpeed;
                        GameObject newBullet = Instantiate(bulletTemplate);
                        newBullet.SetActive(true);
                        couldShoot = true;
                        shootOnce = true;

                        if(OnShootMeteor != null)
                        {
                            OnShootMeteor();
                            if(secondTime)
                            {
                                if (OnShootMeteorSecond != null)
                                {
                                    OnShootMeteorSecond();
                                }
                            }
                        }

                        AkSoundEngine.PostEvent("meteoro_lanza", shootMeteorSound);
                        Debug.Log("METEOR SHOOT");
                        secondTime = true;
                    }
                }
            }

            if(couldShoot)
            {
                currentMeteors--;
                UpdateText();
            }

            if(currentMeteors == 1)
            {
                AkSoundEngine.PostEvent("meteoro_ultimo", lastMeteorSound);
                Debug.Log("LAST METEOR");
            }
        }
        else
        {
            AkSoundEngine.PostEvent("meteoro_no", unavailableSound);
        }

        return couldShoot;
    }

    public void SwitchActivation()
    {
        if(GameManager.Get().turretSpawner.preview)
        {
            GameManager.Get().SwitchTurretActivation();
        }

        isActivated = !isActivated;

        if (isActivated)
        {
            explosionRadius.SetActive(true);
            meteorButton.image.color = enableColor;
            uiAnimation.ChangeGradientElement(0, 0);
        }
        else
        {
            explosionRadius.SetActive(false);
            meteorButton.image.color = disableColor;
            uiAnimation.ChangeGradientElement(0, 2);
        }
    }

    public void SwitchActivationForced()
    {
        isActivated = !isActivated;

        if (isActivated)
        {
            explosionRadius.SetActive(true);
            meteorButton.image.color = enableColor;
            uiAnimation.ChangeGradientElement(0, 0);
        }
        else
        {
            explosionRadius.SetActive(false);
            meteorButton.image.color = disableColor;
            uiAnimation.ChangeGradientElement(0, 2);
        }
    }

    private void UpdateText()
    {
        meteorCountText.text = "X" + currentMeteors;
    }

    private void SetMouseOverOff()
    {
        isMouseOver = false;
    }

    private void SetMouseOverOn()
    {
        isMouseOver = true;
    }

    public void Reload()
    {
        rechargeTimer = 0;
        currentMeteors++;
        meteorButton.image.fillAmount = 1;
        uiAnimation.ExecuteCurves();
        UpdateText();
        AkSoundEngine.PostEvent("meteoro_cooldown", cooldownSound);
    }

    private void PreviewRadius()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray.origin, ray.direction, out hit, 999, Mask))
        {

            if (hit.transform.gameObject.tag == "planet")
            {
                explosionRadius.transform.position = hit.point + (hit.normal * 1);
                Quaternion newRot = Quaternion.FromToRotation(Vector3.up, hit.normal);
                explosionRadius.transform.rotation = Quaternion.Slerp(explosionRadius.transform.rotation, newRot,Time.deltaTime * 12);
            }
        }
        else
        {
            explosionRadius.transform.position = ray.origin * -3;
        }

        /*if (turretProperties.canBePlaced && turretProperties.isInTurretZone)
        {
            material.SetColor("_BaseColor", Color.green);
            turretMaterial.SetPropertyBlock(material);
        }
        else
        {
            material.SetColor("_BaseColor", Color.red);
            turretMaterial.SetPropertyBlock(material);
        }*/
    }

    public void TurnSecondTimeON()
    {
        secondTime = true;
    }

    private void OnDestroy()
    {
        TutorialEvents.OnEventMeteorClose -= TurnSecondTimeON;
    }
}
