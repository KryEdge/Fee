using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shoot : MonoBehaviour
{
    public Button meteorButton;
    public Text meteorCountText;
    public GameObject bulletTemplate;
    public float fireRate;
    public float rechargeTime;
    public float rechargeTimer;

    public LayerMask Mask;
    public int maxMeteors;
    public int currentMeteors;

    private Bullet bulletProperties;
    private bool shootOnce;
    private bool canShoot;
    public bool isActivated;
    public bool isMouseOver;
    private float fireRateTimer;
    // Start is called before the first frame update
    void Start()
    {
        bulletProperties = bulletTemplate.GetComponent<Bullet>();
        //UIMeteor.OnMouseOverButton = SetMouseOverOn;
        //UIMeteor.OnMouseExitButton = SetMouseOverOff;

        currentMeteors = maxMeteors;

        UpdateText();
    }

    private void Update()
    {
        if(currentMeteors < maxMeteors)
        {
            rechargeTimer += Time.deltaTime;

            float fill = ((rechargeTimer * 100) / rechargeTime) * 0.01f;

            meteorButton.image.fillAmount = fill;

            if (rechargeTimer >= rechargeTime)
            {
                rechargeTimer = 0;
                currentMeteors++;
                meteorButton.image.fillAmount = 1;
                UpdateText();
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
                if (hit.transform.gameObject.tag != "explosion" && Time.timeScale != 0)
                {
                    if (!shootOnce)
                    {
                        Debug.Log(hit.transform.gameObject.tag);
                        bulletProperties.isFired = true;
                        bulletProperties.target = hit.point;
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
        if(!GameManager.Get().turretSpawner.preview)
        {
            isActivated = !isActivated;
        }

        if(isActivated)
        {
            meteorButton.image.color = Color.green;
        }
        else
        {
            meteorButton.image.color = Color.cyan;
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
}
