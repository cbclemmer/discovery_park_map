using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using System;
using System.Text.RegularExpressions;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using System.Linq;
using System.IO;

public class Main_Controller : MonoBehaviour
{
    // delete later
    public UI_Controller UIController { get => GetComponent<UI_Controller>(); }
    public Node_controller Start_Node;
    public Node_controller End_Node;

    public List<Node_controller> Nodes;
    public List<Node_controller> Cur_Path; //list to store the current path in memory
    // Start is called before the first frame update
[Serializable]
    public enum App_State{
        Splash,
        Map,
        Search,
        SearchWStart,
        Confirm,
        Route, 
        Cancel
    }
    public App_State State = App_State.Splash;
    public App_State state{
        get{
            return State;
        }
        set{
            State = value;
            if(value == App_State.Splash){
                Debug.Log("App state Changed to Splash");
            }
            if(value == App_State.Map){
                Debug.Log("App state Changed to Map");
            }
            if(value == App_State.Search){
                Debug.Log("App state Changed to Search");
            }
            if(value == App_State.SearchWStart){
                Debug.Log("App state Changed to SearchWStart");
            }
            if(value == App_State.Confirm){
                Debug.Log("App state Changed to Confirm");
            }
            if(value == App_State.Route){
                Debug.Log("App state Changed to Route");
            }
            if(value == App_State.Cancel){
                Debug.Log("App state Changed to Cancel");
            }
        }
    }
    void Start()
    {
        Nodes = GameObject
            .FindGameObjectsWithTag("Node")
            .Select(o => o.GetComponent<Node_controller>())
            .ToList();

        var i = 0;
        foreach (var node in Nodes)
        {
            node.Id = i;
            i++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public List<Node_controller> Find_Path(Node_controller start, Node_controller end) 
    {
       return Path_Controller.FindShortestPath(start,end, Nodes);
    }
    
    
    public List<Node_controller> Search_Nodes(string search){
        Regex regex = new Regex($"^{Regex.Escape(search.ToLower())}");
        var results = new List<Node_controller>();
        for (int i = 0; i < Nodes.Count; i++){
            if(regex.Match(Nodes[i].Name.ToLower()).Success){
                results.Add(Nodes[i]);
            }
        }
        return results;
    }
    

    public int GetWalkTime() { //converts path distance into walktime, 1 unity unit = 1 meter
        if (Cur_Path == null){ 
            return int.MaxValue;
        }
        float sum = 0;
        for(int i = 0; i < Cur_Path.Count -1; i++){
            sum += (Cur_Path[i].transform.position - Cur_Path[i+1].transform.position).magnitude *7.62f;
        }
        float walkTime = sum/1.338f; //average walking speed is 1.388 meters per second
        return (int)(walkTime); //give time in minutes 
    }
}
