using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Click_Controller : MonoBehaviour, IPointerClickHandler
{
    public Func<int> F;
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Test");
    }

}

