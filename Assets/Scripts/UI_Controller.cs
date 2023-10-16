using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class UI_Controller : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI TimeText;
    public Main_Controller MainController { get => GetComponent<Main_Controller>(); }

    public List<float> ZoomLevels = new List<float>
    {
        6,7,8,
        9,10,11
    };
    public int ZoomLevel = 2;
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

    public void Draw_Path(List<Node_controller> path)
    {
        Remove_Path();
        for(int i=0; i < path.Count -1; i++)
        {
            path[i].Draw_Line(path[i + 1]);
            
        }
        MainController.Cur_Path = path;
        int walkTime = MainController.GetWalkTime();
        SetWalkTime(walkTime);
    }

    public void SetWalkTime(int walkTime){
         Debug.Log(walkTime);
         TimeText.text = $"Walk Time: {walkTime/60} min {walkTime%60} sec";  
    }

    public void Remove_Path(){
        if(GetComponent<Main_Controller>().Cur_Path == null){
            return;
        }
        for(int i=0; i <  MainController.Cur_Path.Count -1; i++){
            MainController.Cur_Path[i].Remove_Line();
        }
        MainController.Cur_Path = null;
    }
}
