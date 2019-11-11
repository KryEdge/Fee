using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Shoot : MonoBehaviour
{
    [Header("General Settings")]
    public KeyCode activateKey;
    public float fireRate;
    public float rechargeTime;
    public float meteorSpeed;
    public int maxMeteors;
    public LayerMask Mask;

    [Header("Assign Components")]
    public GameObject bulletTemplate;

    [Header("UI Settings")]
    public Button meteorButton;
    public Text meteorCountText;
    public Color disableColor;
    public Color enableColor;

    [Header("Checking Variables")]
    public bool isActivated;

    private Bullet bulletProperties;
    private bool shootOnce;
    private bool canShoot;
    private bool isMouseOver;
    private int currentMeteors;
    private float fireRateTimer;
    private float rechargeTimer;
    // Start is called before the first frame update
    void Start()
    {
        bulletProperties = bulletTemplate.GetComponent<Bullet>();

        currentMeteors = maxMeteors;

        UpdateText();
        SwitchActivation();
    }

    private void Update()
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
                if(isActivated && !GameManager.Get().turretSpawner.preview && !isMouseOver)
                {
                    if(ShootWeapon())
                    {
                        shootOnce = false;
                        canShoot = false;
                    }
                }
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
                    }
                }
            }

            if(couldShoot)
            {
                currentMeteors--;
                UpdateText();
            }
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
            meteorButton.image.color = enableColor;
        }
        else
        {
            meteorButton.image.color = disableColor;
        }
    }

    public void SwitchActivationForced()
    {
        isActivated = !isActivated;

        if (isActivated)
        {
            meteorButton.image.color = enableColor;
        }
        else
        {
            meteorButton.image.color = disableColor;
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
        UpdateText();
    }
}
