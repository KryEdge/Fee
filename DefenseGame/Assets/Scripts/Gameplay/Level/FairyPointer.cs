using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyPointer : MonoBehaviour
{
    public GameObject target;
    public GameObject uiPointer;
    public float offset;

    private Vector3 targetPosition;
    private RectTransform pointerRectTransform;

    // Start is called before the first frame update
    private void Start()
    {
        target = FlockManager.fairies[0];
        targetPosition = target.transform.position;
        pointerRectTransform = uiPointer.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    private void Update()
    {
        target = FlockManager.fairies[0];
        targetPosition = target.transform.position;

        Vector3 toPosition = targetPosition;
        Vector3 fromPosition = Camera.main.transform.position;
        fromPosition.z = 0;

        Vector3 dir = (toPosition - fromPosition).normalized;
        float angle = GetAngleFromVectorFloat(dir);

        pointerRectTransform.localEulerAngles = new Vector3(0,0,angle);
    }

    private float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }
}
