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
    public GameObject selection;
    public GameObject cancel_button;
    public void OnPointerClick(PointerEventData eventData)
    {
        /*Debug.Log("Test");
        Debug.Log(main_controller.State);
        */
        main_controller.state = Main_Controller.App_State.Search;
        //Debug.Log(main_controller.State);
        selection.SetActive(true);
        cancel_button.SetActive(true);
    }
    public void CancelSearch(){
        main_controller.state = Main_Controller.App_State.Map;
    }
    public void Start(){
        search_bar.onValueChanged.AddListener(delegate{InputUpdate();});
    }
    public void InputUpdate(){
        //Debug.Log(search_bar.text);
        class_results = main_controller.Search_Nodes(search_bar.text);
    }

}
