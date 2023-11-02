using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Search_Controller : MonoBehaviour
{
    public Main_Controller main_controller;
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

        for(int i = 0;i < 5; i++){
            var textLabel = button_results[i].GetComponentInChildren<TMP_Text>();
            if(search_bar.text != ""){
                if(i < class_results.Count){
                    textLabel.text = class_results[i].name;
                }
                else{
                    textLabel.text = "";
                }
            }
            else{
                textLabel.text = "";
            }
        }
    }

}
