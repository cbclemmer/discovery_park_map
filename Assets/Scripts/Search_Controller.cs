using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

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

        if (class_results.Count == 0)
        {
            SetStart(main_controller.Start_Node);
            return;
        }

        var hasStart = main_controller.Start_Node != null;
        var startIndex = hasStart ? 1 : 0;
        for(int i = startIndex;i < 5; i++){
            var textLabel = button_results[i].GetComponentInChildren<TMP_Text>();
            var idx = hasStart ? i - 1 : i;
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
        if (search_bar.text.Equals(string.Empty)) {
            switch (index)
            {
                case 1:
                    _searchForTapDestination();
                    break;
                case 2:
                    _setNearestGeneral("Exit");
                    break;
                case 3:
                    _setNearestGeneral("Fountain");
                    break;
                case 4:
                    _setNearestGeneral("Bathroom");
                    break;
            }
            return;
        }
        if (main_controller.Start_Node != null) index -= 1;
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

    private void _setNearestGeneral(string name)
    {
        Node_controller closestGeneral = null;
        var minDist = int.MaxValue;
        foreach(var node in main_controller.Nodes.Where(n => n.Type.Equals(Node_controller.NodeType.General) && n.Name.Equals(name)))
        {
            var path = main_controller.Find_Path(main_controller.Start_Node, node);
            var walkTime = main_controller.GetWalkTime(path);
            if (walkTime < minDist)
            {
                closestGeneral = node;
                minDist = walkTime;
            }
        }
        tappedEnd(closestGeneral);
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

        textBox = button_results[2].GetComponentInChildren<TMP_Text>();
        textBox.text = "Nearest Exit";
        button_results[2].SetActive(true);

        textBox = button_results[3].GetComponentInChildren<TMP_Text>();
        textBox.text = "Nearest Fountain";
        button_results[3].SetActive(true);

        textBox = button_results[4].GetComponentInChildren<TMP_Text>();
        textBox.text = "Nearest Restroom";
        button_results[4].SetActive(true);
    }
}
