using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using TMPro;

public class Click_Controller : MonoBehaviour, IPointerClickHandler
{
    public Main_Controller main_controller;
    public TMP_InputField search_bar;
    public List<Node_controller> class_results;
    public void OnPointerClick(PointerEventData eventData)
    {
        /*Debug.Log("Test");
        Debug.Log(main_controller.State);
        */
        main_controller.State = Main_Controller.App_State.Search;
        //Debug.Log(main_controller.State);
        main_controller.selection.SetActive(true);
    }
    public void Start(){
        search_bar.onValueChanged.AddListener(delegate{InputUpdate();});
    }
    public void InputUpdate(){
        //Debug.Log(search_bar.text);
        class_results = main_controller.Search_Nodes(search_bar.text);
    }

}
