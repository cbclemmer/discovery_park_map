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
    public int Id;
    public string Name;
    public List<Node_controller> Connections;
    public NodeType Type;
    public Sprite PathSprite;
    // Start is called before the first frame update
    void Start()
    {
        if (Type.Equals(NodeType.Pathing)) {
            GetComponent<SpriteRenderer>().sprite = PathSprite;
        }
        Name = gameObject.name;
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
    public void Draw_Line(Node_controller destination)
    {
        var lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, destination.transform.position);
    }

    public void Remove_Line(){
        var lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;
    }
}
