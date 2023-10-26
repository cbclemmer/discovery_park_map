using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using TMPro;

public class Search_Controller : MonoBehaviour, IPointerClickHandler
{
    public Main_Controller main_controller;
    public TMP_InputField search_bar;
    private List<Node_controller> class_results;
    public List<GameObject> buttons_results;
    public void OnPointerClick(PointerEventData eventData)
    {
        /*Debug.Log("Test");
        Debug.Log(main_controller.State);
        */
        main_controller.state = Main_Controller.App_State.Search;
        //Debug.Log(main_controller.State);
    }
    public void CancelSearch(){
        main_controller.state = Main_Controller.App_State.Map;
    }
    public void Start(){
        class_results = new List<Node_controller>();
        search_bar.onValueChanged.AddListener(delegate{InputUpdate();});
    }
    public void InputUpdate(){
        //Debug.Log(search_bar.text);
        class_results = main_controller.Search_Nodes(search_bar.text);
        int class_size = class_results.Count;
        if(class_size > 5)
            class_size = 5;

        for(int i = 0;i < class_size; i++)
            buttons_results[i].GetComponentInChildren<Text>().text = class_results[i].name;
    }

}
