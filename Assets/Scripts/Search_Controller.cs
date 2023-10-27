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
    public List<GameObject> button_results;
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
        search_bar.onValueChanged.AddListener(delegate{InputUpdate();});
    }
    public void InputUpdate(){
        //Debug.Log(search_bar.text);
        class_results = main_controller.Search_Nodes(search_bar.text);
        int class_size = class_results.Count;

        for(int i = 0;i < 5; i++){
            if(i < class_size){
                button_results[i].GetComponentInChildren<TMP_Text>().text = class_results[i].name;
            }
            else{
                button_results[i].GetComponentInChildren<TMP_Text>().text = "";
            }
        }
    }

}
