using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Controller : MonoBehaviour
{
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

}
