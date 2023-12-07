using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Controller : MonoBehaviour
{
    public GameObject SplashState;
    public GameObject MapState;
    public GameObject ConfirmState;
    public GameObject SearchState;
    public GameObject RouteState;
    public GameObject ConfirmNodeState;
    

    public Sprite Start_Icon; 
    public GameObject Floor_One_Sprite;
    public GameObject Floor_Two_Sprite;

    [SerializeField] public TextMeshProUGUI TimeText;
    public Main_Controller MainController { get => GetComponent<Main_Controller>(); }
    public Search_Controller search_controller;
    public Confirm_Node_Controller ConfirmNodeController;
    public List<float> ZoomLevels = new List<float>
    {
        6,7,8,
        9,10,11
    };
    public int ZoomLevel = 2;
    public float ClickNodeDistance;
    public int MaxClickFrames;
    private int _currentClickFrames;

    void Start()
    {
        ZoomLevel = ZoomLevels.Count - 1;
        UpdateZoom();
    }

    void Update()
    {
        //checks if mouse is clicked
        if(Input.GetMouseButton(0)) {
            _currentClickFrames++;
        } else {
            if (_currentClickFrames > 0 && _currentClickFrames < MaxClickFrames) {
                _handleClick();
            }
            _currentClickFrames = 0;
        }
        //scroll to zoom
        if(Input.GetAxis("Mouse ScrollWheel") > 0){ //zoom in
            ZoomIn();
        }
        if(Input.GetAxis("Mouse ScrollWheel") < 0){ //zoom out
            ZoomOut();
        }
        //Scroll to zoom
        if(Input.touchCount == 2){ //check if two fingers on
            float distance;
            float new_distance;
            var touch_one = Input.GetTouch(0).position; //tracker for first touch
            var touch_two = Input.GetTouch(1).position; //tracker for second touch
            distance = Vector2.Distance(touch_one, touch_two);
            while(Input.touchCount == 2){ //track the position of two touches and if distance increases or decreases
                touch_one = Input.GetTouch(0).position; //tracker for first touch
                touch_two = Input.GetTouch(1).position;
                new_distance = Vector2.Distance(touch_one, touch_two);
                if(new_distance > distance){
                    ZoomOut();
                    distance = new_distance;
                }
                if(new_distance < distance){
                    ZoomIn();
                    distance = new_distance;
                }
            }
        }
        

    }

    private void _checkNodeClick(Vector3 mousePos)
    {
        Node_controller clickedNode = null;
        var minDist = float.MaxValue;
        foreach(var node in MainController.Nodes)
        {
            if (node.Name == string.Empty) continue; 
            var distance = (mousePos - node.transform.position).magnitude;
            if(distance < ClickNodeDistance && distance < minDist) {
                minDist = distance;
                clickedNode = node;
            }
        }

        if (clickedNode == null) return;

        if(MainController.State == Main_Controller.App_State.Map) { //set start clickedNode
            ConfirmNodeController.HandleStartNodeClick(clickedNode);
        }
        else{ //set end clickedNode and go to confirmation screen
            // TODO: Handle destination node click
            return;
        }
    }

    private void _handleClick()
    {
        //declare raycast for location of click
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        _checkNodeClick(mousePos);
    }


    public void ZoomIn()
    {
        if (ZoomLevel == 0)
            return;
        ZoomLevel--;
        UpdateZoom();
    }
    public void ZoomOut()
    {
        if (ZoomLevel == ZoomLevels.Count - 1)
            return;
        ZoomLevel++;
        UpdateZoom();
    }
    public void UpdateZoom()
    {
        GetComponent<Camera>().orthographicSize = ZoomLevels[ZoomLevel];
    }

    public void ResetZoom()
    {
        ZoomLevel = ZoomLevels.Count - 1;
        UpdateZoom();
    }

    public void Create_Path()
    {
        if (MainController.Start_Node == null || MainController.End_Node == null)
        {
            throw new System.Exception("UI::Create_Path: Main controller start node or end node is not set");
        }

        var path = MainController.Find_Path(MainController.Start_Node, MainController.End_Node);
        MainController.Cur_Path = path;
        Draw_Path(path);
    }

    public void Draw_Path(List<Node_controller> path)
    {
        Remove_Path();
        for(int i=0; i < path.Count -1; i++)
        {
            path[i].Draw_Line(path[i + 1]);
        }
    }

    public string GetWalkTimeString()
    {
        var walkTime = MainController.GetWalkTime();
        return $"{walkTime/60} min {walkTime%60} sec";
    }

    public void SetWalkTime(){
         TimeText.text = $"Walk Time: {GetWalkTimeString()}";
    }

    public void Remove_Path(){
        if(GetComponent<Main_Controller>().Cur_Path == null){
            return;
        }
        for(int i=0; i <  MainController.Cur_Path.Count -1; i++){
            MainController.Cur_Path[i].Remove_Line();
        }
    }

    public void Change_State(Main_Controller.App_State state, bool resetView = true){
        MainController.State = state;
        if (resetView) {
            Reset_view();
        }
        SplashState.SetActive (state==Main_Controller.App_State.Splash);
        MapState.SetActive (state==Main_Controller.App_State.Map);
        ConfirmState.SetActive (state==Main_Controller.App_State.Confirm);
        SearchState.SetActive (state==Main_Controller.App_State.Search);
        ConfirmNodeState.SetActive (state==Main_Controller.App_State.ConfirmNode);
        RouteState.SetActive (state==Main_Controller.App_State.Route);
    }

    public void Set_Map_State(){
        Change_State(Main_Controller.App_State.Map);
    }

    public void Set_Search_State(){
        Change_State(Main_Controller.App_State.Search);
    }
    
    public void Change_Floor(int floor)
    {
        if (floor < 1 || floor > 2) return;
        MainController.CurrentFloor = floor;
        Floor_One_Sprite.SetActive(floor == 1);
        Floor_Two_Sprite.SetActive(floor == 2);
    }

    public void Reset_view()
    {
        Camera.main.transform.position = new Vector3(0, 0, -10);
        Camera.main.transform.rotation = Quaternion.identity;
        ResetZoom();
        Change_Floor(1);
    }
}
