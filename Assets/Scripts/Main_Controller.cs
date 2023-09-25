using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class Main_Controller : MonoBehaviour
{
    // delete later
    public Node_controller Start_Node;
    public Node_controller End_Node;

    public List<Node_controller> Nodes;
    public List<Node_controller> Cur_Path; //list to store the current path in memory
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("hello, world");
        Draw_Path(new List<Node_controller> { Start_Node, End_Node });
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
    
    
}
