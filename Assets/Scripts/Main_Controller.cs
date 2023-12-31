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
    public GameObject NodePrefab;
    public bool TestConnections;
    public float PathInterval;

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

        var pathNodes = Nodes.Where(n => n.Type.Equals(Node_controller.NodeType.Pathing)).ToList();
        // Ensure nodes are connected both ways
        foreach (var node in pathNodes)
        {
            node.Connections = node.Connections.Where(c => c != null).ToList();
            if (node.Type.Equals(Node_controller.NodeType.Pathing))
            {
                foreach (var connection in node.Connections)
                {
                    if (connection != null && connection.Connections != null 
                        && !connection.Connections.Any(c => c != null && c.Id == node.Id)) 
                    {
                        connection.Connections.Add(node);
                    }
                }
            }
        }

        var visited = new List<KeyValuePair<int, int>>();
        foreach (var node in pathNodes)
        {
            var connectionArr = new Node_controller[node.Connections.Count];
            node.Connections.CopyTo(connectionArr);
            foreach(var connection in connectionArr)
            {
                if (connection.Type != Node_controller.NodeType.Pathing) continue;
                if (visited.Any(v => (v.Key == node.Id && v.Value == connection.Id) || (v.Key == connection.Id && v.Value == node.Id))) continue;
                visited.Add(new KeyValuePair<int, int>(node.Id, connection.Id));
                var lastNode = node;
                var vec = connection.transform.position - node.transform.position;
                var connectionDist = vec.magnitude;
                var pos = node.transform.position + (vec * (PathInterval / vec.magnitude));
                var dist = (node.transform.position - pos).magnitude;
                while (dist < connectionDist)
                {
                    var newNode = Instantiate(NodePrefab).GetComponent<Node_controller>();
                    newNode.Type = Node_controller.NodeType.Pathing;
                    newNode.transform.position = pos;
                    newNode.Id = currentId;
                    currentId++;
                    newNode.Connections.Add(lastNode);
                    lastNode.Connections.Add(newNode);
                    newNode.transform.SetParent(node.transform);
                    Nodes.Add(newNode);
                    lastNode = newNode;
                    vec = connection.transform.position - lastNode.transform.position;
                    pos = lastNode.transform.position + (vec * (PathInterval / vec.magnitude));
                    dist = (node.transform.position - pos).magnitude;
                }

                if (lastNode.Id != node.Id)
                {
                    connection.Connections.Add(lastNode);
                    lastNode.Connections.Add(connection);
                    node.Connections.Remove(connection);
                    connection.Connections.Remove(node);
                }
            }
        }

        pathNodes = Nodes.Where(n => n.Type.Equals(Node_controller.NodeType.Pathing)).ToList();
        foreach (var node in Nodes.Where(n => !n.Type.Equals(Node_controller.NodeType.Pathing)))
        {
            node.Connections = new List<Node_controller>();
            Node_controller closestPathNode = null;
            var minDist = float.MaxValue;
            foreach(var pathNode in pathNodes)
            {
                var dist = (pathNode.transform.position - node.transform.position).magnitude;
                if (dist < minDist) 
                {
                    minDist = dist;
                    closestPathNode = pathNode;
                }
            }
            if (closestPathNode != null)
            {
                closestPathNode.Connections.Add(node);
                node.Connections.Add(closestPathNode);
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
            if(Nodes[i].name != string.Empty && regex.Match(Nodes[i].Name.ToLower()).Success){
                results.Add(Nodes[i]);
            }
        }
        return results;
    }
    

    public int GetWalkTime(List<Node_controller> path = null) { //converts path distance into walktime, 1 unity unit = 1 meter
        if (path == null) path = Cur_Path;
        if (path == null){ 
            throw new Exception("UI::GetWalkTimeString: Current path is not set");
        }
        float sum = 0;
        for(int i = 0; i < path.Count -1; i++){
            sum += (path[i].transform.position - path[i+1].transform.position).magnitude *7.62f;
        }
        float walkTime = sum/1.338f; //average walking speed is 1.388 meters per second
        return (int)(walkTime); //give time in minutes 
    }
}

