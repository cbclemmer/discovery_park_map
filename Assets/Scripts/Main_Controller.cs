using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using System;
using System.Text.RegularExpressions;
using UnityEngine.Rendering;

public class Main_Controller : MonoBehaviour
{
    // delete later
    public Node_controller Start_Node;
    public Node_controller End_Node;

    public List<Node_controller> Nodes;
    public List<Node_controller> Cur_Path; //list to store the current path in memory
    // Start is called before the first frame update

    public enum App_State{
        Splash,
        Search,
        SearchWStart,
        Confirm,
        Route, 
        Cancel
    }
    public App_State State = App_State.Splash;
    void Start()
    {
        Debug.Log("hello, world");
        Draw_Path(new List<Node_controller> { Start_Node, End_Node });
        Debug.Log(Search_Nodes("S").Count);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public List<Node_controller> Find_Path(Node_controller start, Node_controller end) 
    {
       return PathController.FindShortestPath(start,end, Nodes);
    }
    public void Draw_Path(List<Node_controller> path)
    {
        for(int i=0; i < path.Count -1; i++)
        {
            path[i].Draw_Line(path[i + 1]);
            
        }
        Cur_Path = path;
    }


    public void Remove_Path(){
        if(Cur_Path == null){
            return;
        }
        for(int i=0; i <Cur_Path.Count -1; i++){
            Cur_Path[i].Remove_Line();
        }
        Cur_Path = null;
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
    
}
