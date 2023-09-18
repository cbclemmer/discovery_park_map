using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Main_Controller : MonoBehaviour
{
    public List<Node_controller> Nodes;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("hello, world");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public List<Node_controller> Find_Path(Node_controller start, Node_controller end) 
    {
        var path = new List<Node_controller>();  
        for(int i=0; i < start.Connections.Count; i++){
            if(start.Connections[i].name == end.name){
                path.Add(start.Connections[i]);
            }
        }
        return path;
    }
    
}
