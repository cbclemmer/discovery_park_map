using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text.RegularExpressions;
using System.Linq;

public class Main_Controller : MonoBehaviour
{
    // delete later
    public UI_Controller UIController { get => GetComponent<UI_Controller>(); }
    public Node_controller Start_Node;
    public Node_controller End_Node;

    public List<Node_controller> Nodes;
    public bool TestConnections;

    //list to store the current path in memory
    public List<Node_controller> Cur_Path; 
    
    public int CurrentFloor;
    public enum App_State{
        Splash,
        Map,
        Search,
        ConfirmNode,
        Confirm,
        Route, 
        Cancel
    }

    public App_State State = App_State.Splash;
    
    void Start()
    {
        Nodes = GameObject
            .FindGameObjectsWithTag("Node")
            .Select(o => o.GetComponent<Node_controller>())
            .ToList();

        
        // Set up ids
        var currentId = 0;
        foreach (var node in Nodes)
        {
            node.Id = currentId;
            currentId++;
        }

        // Ensure nodes are connected both ways
        foreach (var node in Nodes)
        {
            node.Connections = node.Connections.Where(c => c != null).ToList();
            foreach (var connection in node.Connections)
            {
                if (connection != null && connection.Connections != null 
                    && !connection.Connections.Any(c => c != null && c.Id == node.Id)) 
                {
                    connection.Connections.Add(node);
                }
            }
        }

        // View all of the connected nodes
        if (TestConnections) {
            foreach(var node in Nodes)
            {
                var lineRenderer = node.GetComponent<LineRenderer>();
                lineRenderer.positionCount = node.Connections.Count * 3;
                for (var i = 0; i < node.Connections.Count; i++) 
                {
                    var connection = node.Connections[i];
                    if (connection == null) continue;
                    var j = i * 3;
                    lineRenderer.SetPosition(j, node.transform.position);
                    lineRenderer.SetPosition(j + 1, connection.transform.position);
                    lineRenderer.SetPosition(j + 2, node.transform.position);
                }
            }
        }
    }
    
    public List<Node_controller> Find_Path(Node_controller start, Node_controller end) 
    {
       return Path_Controller.FindShortestPath(start,end, Nodes);
    }
    
    public List<Node_controller> Search_Nodes(string search){
        Regex regex = new Regex($"^{Regex.Escape(search.ToLower())}");
        var results = new List<Node_controller>();
        for (int i = 0; i < Nodes.Count; i++){
            if (Start_Node != null && Nodes[i].Id == Start_Node.Id) continue;
            if(Nodes[i].transform.parent.name == "Room_Nodes" && regex.Match(Nodes[i].Name.ToLower()).Success){
                results.Add(Nodes[i]);
            }
        }
        return results;
    }
    

    public int GetWalkTime() { //converts path distance into walktime, 1 unity unit = 1 meter
        if (Cur_Path == null){ 
            throw new Exception("UI::GetWalkTimeString: Current path is not set");
        }
        float sum = 0;
        for(int i = 0; i < Cur_Path.Count -1; i++){
            sum += (Cur_Path[i].transform.position - Cur_Path[i+1].transform.position).magnitude *7.62f;
        }
        float walkTime = sum/1.338f; //average walking speed is 1.388 meters per second
        return (int)(walkTime); //give time in minutes 
    }
}

