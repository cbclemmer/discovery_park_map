using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Splash_controller : MonoBehaviour, IPointerClickHandler
{
    public UI_Controller UIController;
    public void OnPointerClick(PointerEventData eventData)
    {
        UIController.Change_State(Main_Controller.App_State.Map);
        UIController.LastButtonClickTime = DateTime.Now;
    }
}
