using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIMeteor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public delegate void OnMouseAction();
    public static OnMouseAction OnMouseOverButton;
    public static OnMouseAction OnMouseExitButton;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (OnMouseOverButton != null)
        {
            OnMouseOverButton();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (OnMouseExitButton != null)
        {
            OnMouseExitButton();
        }
    }
}
