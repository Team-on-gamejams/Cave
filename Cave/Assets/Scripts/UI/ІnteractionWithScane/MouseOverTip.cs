using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MouseOverTip : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI TipText;

    void Awake()
    {
        EventManager.OnMouseOverTip += OnPopUpShow;
    }

    void OnDestroy()
    {
        EventManager.OnMouseOverTip -= OnPopUpShow;
    }

    void OnPopUpShow(EventData ed)
    {
        TipText.text = ((string)(ed?["tipText"])) ?? "MISSING TEXT";
        transform.position = Input.mousePosition;

        LeanTween.cancel(gameObject);
        LeanTween.delayedCall(gameObject, 0.05f, PopUpHide);
    }

    void PopUpHide()
    {
        TipText.text = "";
    }
}
