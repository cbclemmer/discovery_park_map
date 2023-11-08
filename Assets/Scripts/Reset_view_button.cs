using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reset_view_button : MonoBehaviour
{
    public Transform Camera;

    // Update is called once per frame
    public void Reset_view()
    {
        Camera.position = new Vector3(0, 0, -10);
        Camera.rotation = Quaternion.identity;
    }
}
