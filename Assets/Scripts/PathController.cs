using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class Dijkstra : MonoBehaviour
{
    public class Node
    {
        public Node_controller node;
        public List<Edge> edges;
        public float distance = float.MaxValue;
        public Node previousNode = null;
        public bool visited = false;
    }

    [Serializable]
    public class Edge
    {
        public Node_controller targetNode_controller;
    }

    public List<Node_controller> Node_controllers;

    private void Start()
    {
      
    }

    private Node_controller FindNode_controllerByObject(GameObject nodeObject)
    {
        foreach (var Node_controller in Node_controllers)
        {
            if (Node_controller.nodeObject == nodeObject)
                return Node_controller;
        }
        return null;
    }

    public static List<Node_controller> FindShortestPath(Node_controller startNode_controller, Node_controller endNode_controller,List<Node_controller> nodes)
    {
        var path_nodes = new List<Node>();
        
        foreach (var node_controller in nodes)
        {
            var node = new Node();
            node.node = node_controller;
            node.distance = float.MaxValue;
            node.previousNode = null;
            node.visited = false;
            path_nodes.Add(node);
        }
        var start_node = new Node();
        start_node.distance = 0;
        var unvisitedNodes = new List<Node>(path_nodes);

        while (unvisitedNodes.Count > 0)
        {
            var currentNode = GetClosestNode(unvisitedNodes);

            if (currentNode == null)
                break;

            currentNode.visited = true;

            foreach (var edge in currentNode.edges)
            {
                Node_controller targetNode_controller = edge.targetNode_controller;

                if (!targetNode_controller.visited)
                {
                    float tentativeDistance = Vector3.Distance(currentNode.nodeObject.transform.position, targetNode_controller.nodeObject.transform.position);

                    if (tentativeDistance < targetNode_controller.distance)
                    {
                        targetNode_controller.distance = tentativeDistance;
                        targetNode_controller.previousNode = currentNode;
                    }
                }
            }
        }

        return BuildPath(startNode_controller, endNode_controller);
    }

    private static Node GetClosestNode(List<Node> nodeList)
    {
        Node closestNode = null;
        float shortestDistance = float.MaxValue;

        foreach (var node in nodeList)
        {
            if (node.distance < shortestDistance && !node.visited)
            {
                shortestDistance = node.distance;
                closestNode = node;
            }
        }

        return closestNode;
    }

    private List<Node_controller> BuildPath(Node_controller startNode_controller, Node_controller endNode_controller)
    {
        List<Node_controller> path = new List<Node_controller>();
        Node_controller currentNode_controller = endNode_controller;

        while (currentNode_controller != startNode_controller)
        {
            path.Add(currentNode_controller);
            currentNode_controller = currentNode_controller.previousNode;
        }

        path.Add(startNode_controller);
        path.Reverse();

        return path;
    }
}
