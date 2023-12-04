using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Based on Dijkstra's Algorithm
public class Path_Controller : MonoBehaviour
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
        var path_nodes = _build_graph(nodes);
        
        var unvisitedNodes = new List<Node>(path_nodes);
        var currentNode = path_nodes.First(n => n.node.Id == startNode_controller.Id);
        currentNode.distance = 0; // Zero out starting node

        while (currentNode != null)
        {
            _visit_node(currentNode);
            unvisitedNodes.Remove(currentNode);
            if (currentNode.node.Id == endNode_controller.Id) 
            {
                break;
            }
            currentNode = unvisitedNodes.OrderBy(n => n.distance).FirstOrDefault();
        }

        return _get_path(path_nodes.First(n => n.node.Id == endNode_controller.Id));
    }

    private static List<Node> _build_graph(List<Node_controller> node_controllers)
    {
        var path_nodes = node_controllers
            .Select(n => new Node(n))
            .ToList();

        foreach (var node in path_nodes)
        {
            foreach (var connection in node.node.Connections)
            {
                var path_node = path_nodes.First(pn => pn.node.Id == connection.Id);
                node.edges.Add(new Edge{
                    target_node = path_node,
                    distance = (node.node.transform.position - connection.transform.position).magnitude
                });
            }
        }

        return path_nodes;
    }

    private static void _visit_node(Node current_node)
    {
        foreach (var edge in current_node.edges)
        {
            if (edge.target_node.visited) continue;
            var tmp_distance = current_node.distance + edge.distance;
            if (tmp_distance < edge.target_node.distance)
            {
                edge.target_node.distance = tmp_distance;
                edge.target_node.previousNode = current_node;
            }
        }
        current_node.visited = true;
    }

    private static List<Node_controller> _get_path(Node end_node)
    {
        var current_node = end_node;
        var path = new List<Node_controller> { current_node.node };
        while (current_node != null && current_node.distance != 0)
        {
            current_node = current_node.previousNode;
            if (current_node == null) break;
            path.Add(current_node.node);
        }

        path.Reverse();

        return path;
    }
}
