using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Splash_controller : MonoBehaviour, IPointerClickHandler
{
    public UI_Controller UIController;
    public void OnPointerClick(PointerEventData eventData)
    {
        UIController.Change_State(Main_Controller.App_State.Map);
    }
}
