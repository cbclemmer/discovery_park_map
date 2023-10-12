using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class Dijkstra : MonoBehaviour
{
    public class Node
    {
        public Node_controller node;
        public List<Edge> edges = new List<Edge>();
        public float distance = float.MaxValue;
        public Node previousNode = null;
        public bool visited = false;

        public Node(Node_controller r)
        {
            node = r;
        }
    }

    [Serializable]
    public class Edge
    {
        public Node target_node;
        public float distance;
    }

    public List<Node_controller> Node_controllers;

    public static List<Node_controller> FindShortestPath(Node_controller startNode_controller, Node_controller endNode_controller,List<Node_controller> nodes)
    {
        var path_nodes = nodes
            .Select(n => new Node(n))
            .ToList();

        foreach (var node in path_nodes)
        {
            foreach (var connection in node.node.Connections)
            {
                var path_node = path_nodes.First(pn => pn.node.transform.position == connection.transform.position);
                node.edges.Add(new Edge{
                    target_node = path_node,
                    distance = (node.node.transform.position - connection.transform.position).magnitude
                });
            }
        }
        
        var start_node = new Node(startNode_controller);
        start_node.distance = 0;
        var unvisitedNodes = new List<Node>(path_nodes);
        var currentNode = start_node;

        while (unvisitedNodes.Count > 0)
        {
            visit_node(currentNode);
            unvisitedNodes.Remove(currentNode);
            if (currentNode.node == endNode_controller) 
            {
                break;
            }
        }

        return BuildPath(startNode_controller, endNode_controller);
    }

    private static void visit_node(Node current_node)
    {
        foreach (var edge in current_node.edges)
        {
            if (edge.target_node.visited) continue;
            var tmp_distance = current_node.distance + edge.distance;
            if (tmp_distance < edge.target_node.distance)
            {
                edge.target_node.distance = tmp_distance;
            }
        }
        current_node.visited = true;
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
