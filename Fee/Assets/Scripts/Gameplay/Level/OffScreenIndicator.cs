using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OffScreenIndicator : MonoBehaviour
{
    public GameObject target;
    public Image _uiArrow;
    public Image _uiIndicator;
    public Transform _trackingObject;

    // Use this for initialization
    private void Start()
    {
        target = FlockManager.fairies[0];
        Fairy.OnFairyDanger = ChangeIndicatorColor;
        Fairy.OnFairyEscaped = ChangeIndicatorColor;
    }

    // Update is called once per frame
    private void Update()
    {
        if(FlockManager.fairies.Count > 0)
        {
            target = FlockManager.fairies[0];
        }
        
        if(target)
        {
            UpdateOffscrenIndicator(target.transform.position);
        }
    }


    private void UpdateOffscrenIndicator(Vector3 position)
    {
        Vector3 screenpos = Camera.main.WorldToScreenPoint(position);

        if (screenpos.z > 0 && screenpos.x > 0 && screenpos.x < Screen.width && screenpos.y > 0 && screenpos.y < Screen.height)
        {
            // THE OBJECT IS INSIDE VIEW
            _uiIndicator.transform.position = screenpos;
            _uiIndicator.transform.rotation = Quaternion.Euler(0, 0, 0);
            _uiIndicator.enabled = false;
            _uiArrow.enabled = false;
        }
        else
        {
            _uiIndicator.enabled = false;
            _uiArrow.enabled = true;

            if (screenpos.z < 0)
            {
                screenpos.z *= -1;
            }

            Vector3 screenCenter = new Vector3(Screen.width, Screen.height, 0) / 2;

            screenpos -= screenCenter;

            float angle = Mathf.Atan2(screenpos.y, screenpos.x);
            angle -= 90 * Mathf.Deg2Rad;

            float cos = Mathf.Cos(angle);
            float sin = -Mathf.Sin(angle);

            screenpos = screenCenter + new Vector3(sin * 150, cos * 150, 0);

            float m = cos / sin;

            Vector3 screenBounds = screenCenter * 0.9f;

            if(cos > 0)
            {
                screenpos = new Vector3(screenBounds.y/m, screenBounds.y, 0);
            }
            else
            {
                screenpos = new Vector3(-screenBounds.y/m,-screenBounds.y,0);
            }

            if(screenpos.x > screenBounds.x)
            {
                screenpos = new Vector3(screenBounds.x, screenBounds.x*m,0);
            }
            else if(screenpos.x < -screenBounds.x)
            {
                screenpos = new Vector3(-screenBounds.x, -screenBounds.x*m,0);
            }

            screenpos += screenCenter;

            _uiArrow.transform.position = screenpos;
            _uiArrow.transform.rotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg );
        }
    }

    private void ChangeIndicatorColor(Color newColor)
    {
        _uiArrow.color = newColor;
    }
}
