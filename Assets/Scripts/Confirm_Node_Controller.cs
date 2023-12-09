using UnityEngine;
using TMPro;
using System;

public class Confirm_Node_Controller : MonoBehaviour
{
    public TMP_Text ConfirmText;
    public TMP_Text RoomText;
    public UI_Controller UIController;

    public float SnapCameraYOffset;

    private Node_controller _confirmNode = null;

    private enum ConfirmNodeType
    {
        NotConfirming,
        SetStart,
        SetEnd
    }

    private ConfirmNodeType _confirmType;

    public void HandleStartNodeClick(Node_controller node)
    {
        _snapToNode(node);
        _confirmType = ConfirmNodeType.SetStart;
        _confirmNode = node;
        ConfirmText.text = "Set start?";
        RoomText.text = $"Room: {node.Name}";
    }

    public void HandleDestNodeClick(Node_controller node)
    {
        _snapToNode(node);
        _confirmType = ConfirmNodeType.SetEnd;
        _confirmNode = node;
        ConfirmText.text = "Set Destination?";
        RoomText.text = $"Room: {node.Name}";
    }

    public void CancelNodeSelect()
    {
        UIController.LastButtonClickTime = DateTime.Now;
        switch (_confirmType)
        {
            case ConfirmNodeType.NotConfirming:
                return;
            case ConfirmNodeType.SetEnd:
                _resetConfirm();
                UIController.Change_State(Main_Controller.App_State.Search);
                return;
            case ConfirmNodeType.SetStart:
                _resetConfirm();
                UIController.Change_State(Main_Controller.App_State.Map);
                return;
        }
    }

    public void ConfirmButtonFn()
    {
        UIController.LastButtonClickTime = DateTime.Now;
        switch (_confirmType)
        {
            case ConfirmNodeType.NotConfirming:
                return;
            case ConfirmNodeType.SetStart:
                ConfirmStart();
                return;
            case ConfirmNodeType.SetEnd:
                ConfirmDest();
                return;
        }
    }

    public void ConfirmStart()
    {
        UIController.Set_Search_State();
        UIController.search_controller.SetStart(_confirmNode);
        UIController.search_controller.button_results[0].SetActive(true);
        _resetConfirm();
    }

    public void ConfirmDest()
    {
        UIController.search_controller.tappedEnd(_confirmNode);
        _resetConfirm();
    }

    private void _resetConfirm()
    {
        _confirmNode = null;
        _confirmType = ConfirmNodeType.NotConfirming;
    }

    private void _snapToNode(Node_controller node)
    {
        UIController.Change_State(Main_Controller.App_State.ConfirmNode, false);
        var camera_x = node.transform.position.x;//get position of node for zoom in
        var camera_y = node.transform.position.y - SnapCameraYOffset;
        Camera.main.transform.position =  new Vector3(camera_x, camera_y, -10); //zoom on clicked clickedNode
        Camera.main.transform.rotation = Quaternion.identity; //make sure camera is correct
        UIController.ZoomLevel = 1; //set zoom level
        UIController.UpdateZoom();
    }
}
