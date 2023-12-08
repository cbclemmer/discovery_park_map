using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Search_Controller : MonoBehaviour
{
    public Main_Controller main_controller;
    public Confirm_Controller confirm_Controller;
    public TMP_InputField search_bar;
    private List<Node_controller> class_results;
    public List<GameObject> button_results;
    
    public void Start(){
        search_bar.onValueChanged.AddListener(delegate{InputUpdate();});
    }
    public void InputUpdate(){
        class_results = search_bar.text.Equals(string.Empty)
            ? new List<Node_controller>()
            : main_controller.Search_Nodes(search_bar.text);

        var hasStart = main_controller.Start_Node != null;
        var startIndex = hasStart ? 2 : 0;
        for(int i = startIndex;i < 5; i++){
            var textLabel = button_results[i].GetComponentInChildren<TMP_Text>();
            var idx = hasStart ? i - 2 : i;
            button_results[i].SetActive(idx < class_results.Count);
            if(idx < class_results.Count){
                textLabel.text = class_results[idx].name;
            }
            else{
                textLabel.text = "";
            }
        }
    }

    public void ClickSearchOption(int index)
    {
        if (main_controller.Start_Node != null) index -= 2;
        if (index == -1) {
            _searchForTapDestination();
            return;
        }
        if (search_bar.text == null || search_bar.text == string.Empty) return;
        var searchNodes = main_controller.Search_Nodes(search_bar.text);
        if (index > searchNodes.Count - 1) return;
        var clickedNode = searchNodes[index];
        if (main_controller.Start_Node == null)
        {
            SetStart(clickedNode);
        } else {
            confirm_Controller.Set_Confirm_State(clickedNode);
            button_results[0].GetComponentInChildren<TMP_Text>().text = string.Empty;
        }
        search_bar.text = string.Empty;
        InputUpdate();
    }

    private void _searchForTapDestination()
    {
        main_controller.UIController.Set_Map_State();
    }

    public void tappedEnd(Node_controller endNode){ //function for if the end node is selected via tap.
       
         if(main_controller.End_Node == null) {
            confirm_Controller.Set_Confirm_State(endNode); //set the end node in confirm controller
            button_results[0].GetComponentInChildren<TMP_Text>().text = string.Empty; //get string for confirm screen
        }
        search_bar.text = string.Empty;
        InputUpdate(); //update
    }

    public void CancelSearch()
    {
        main_controller.Start_Node = null;
        main_controller.UIController.Set_Map_State();
        search_bar.text = string.Empty;
        button_results[0].GetComponentInChildren<TMP_Text>().text = string.Empty;
        InputUpdate();
    }

    public void SetStart(Node_controller startNode)
    {
        main_controller.Start_Node = startNode;
        var textBox = button_results[0].GetComponentInChildren<TMP_Text>();
        textBox.text = "Start: " + startNode.Name;

        textBox = button_results[1].GetComponentInChildren<TMP_Text>();
        textBox.text = "Tap Destination";
        button_results[1].SetActive(true);
    }
}
