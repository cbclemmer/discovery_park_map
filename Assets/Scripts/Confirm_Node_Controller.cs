using UnityEngine;
using TMPro;

public class Confirm_Node_Controller : MonoBehaviour
{
    public TMP_Text ConfirmText;
    public TMP_Text RoomText;
    public UI_Controller UIController;

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
        UIController.Change_State(Main_Controller.App_State.ConfirmNode, false);
        var camera_x = node.transform.position.x;//get position of node for zoom in
        var camera_y = node.transform.position.y;
        Camera.main.transform.position =  new Vector3(camera_x, camera_y, -10); //zoom on clicked clickedNode
        Camera.main.transform.rotation = Quaternion.identity; //make sure camera is correct
        UIController.ZoomLevel = 1; //set zoom level
        UIController.UpdateZoom();
        _confirmType = ConfirmNodeType.SetStart;
        _confirmNode = node;
        ConfirmText.text = "Set start?";
        RoomText.text = $"Room: {node.Name}";
    }

    public void CancelNodeSelect()
    {
        switch (_confirmType)
        {
            case ConfirmNodeType.NotConfirming:
                return;
            case ConfirmNodeType.SetEnd:
            case ConfirmNodeType.SetStart:
                _confirmNode = null;
                _confirmType = ConfirmNodeType.NotConfirming;
                UIController.Change_State(Main_Controller.App_State.Map);
                return;
        }
    }

    public void ConfirmButtonFn()
    {
        switch (_confirmType)
        {
            case ConfirmNodeType.NotConfirming:
                return;
            case ConfirmNodeType.SetStart:
                ConfirmStart();
                return;
            case ConfirmNodeType.SetEnd:
                return;
        }
    }

    public void ConfirmStart()
    {
        UIController.Set_Search_State();
        UIController.search_controller.SetStart(_confirmNode);
        UIController.search_controller.button_results[0].SetActive(true);
    }
}
