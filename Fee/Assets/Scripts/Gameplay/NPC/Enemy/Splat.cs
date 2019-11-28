using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splat : MonoBehaviour
{
    public AnimationCurve curve;
    public float lifespan;
    public float sizeSpeed;
    public float animationTime;

    private float desiredSize;
    private float lifespanTimer;
    private float sizeTimer;
    private bool canDelete;
    private bool hasReachedSize;

    // Start is called before the first frame update
    void Start()
    {
        desiredSize = transform.localScale.x;
        StartCoroutine(ScaleCurve());
    }

    // Update is called once per frame
    void Update()
    {
        if(!hasReachedSize)
        {
           /* sizeTimer += Time.deltaTime * sizeSpeed;
            transform.localScale = new Vector3(sizeTimer, sizeTimer, sizeTimer);

            if (sizeTimer >= desiredSize)
            {
                sizeTimer = 0;
                transform.localScale = new Vector3(desiredSize, desiredSize, desiredSize);
                hasReachedSize = true;
            }*/
        }
        else
        {
            if(!canDelete)
            {
                lifespanTimer += Time.deltaTime;

                if (lifespanTimer >= lifespan)
                {
                    canDelete = true;
                    sizeTimer = transform.localScale.x;
                }
            }
            else
            {
                sizeTimer -= Time.deltaTime * sizeSpeed;
                transform.localScale = new Vector3(sizeTimer, sizeTimer, sizeTimer);

                if (sizeTimer <= 0)
                {
                    transform.localScale = new Vector3(0, 0, 0);
                    Destroy(gameObject);
                }
            }
            
        }
        
    }

    IEnumerator ScaleCurve()
    {
        float t = 0;

        while (t <= animationTime)
        {
            t += Time.deltaTime;

            float eval = curve.Evaluate(t / animationTime);

            transform.localScale = Vector3.one * eval;
            
            if(t >= animationTime)
            {
                hasReachedSize = true;
            }

            yield return null;
        }
    }
}
