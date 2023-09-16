using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node_controller : MonoBehaviour
{
    public enum NodeType
    {
        Distinct,
        General,
        Pathing
    }
    public string Name;
    public List<Node_controller> Connections;
    public NodeType Type;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
