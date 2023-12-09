using TMPro;
using UnityEngine;

public class Confirm_Controller : MonoBehaviour
{
    public float ConfirmYOffset;

    public UI_Controller UIController;
    public TMP_Text Start_Text;
    public TMP_Text End_Text;
    public TMP_Text Walk_Text;

    private void _centerCamera()
    {
        var startPos = UIController.MainController.Start_Node.transform.position;
        var endPos = UIController.MainController.End_Node.transform.position;
        var vec = .5f * (endPos - startPos);
        var newCameraPos = startPos + vec;
        newCameraPos.y -= ConfirmYOffset;
        newCameraPos.z = -10;
        Camera.main.transform.position = newCameraPos;
    }

    public void Set_Confirm_State(Node_controller endNode) {
        if (UIController.MainController.Start_Node == null) return;
        UIController.MainController.End_Node = endNode;
        UIController.Change_State(Main_Controller.App_State.Confirm);
        Start_Text.text = UIController.MainController.Start_Node.Name;
        End_Text.text = UIController.MainController.End_Node.Name;
        UIController.Create_Path();
        Walk_Text.text = UIController.GetWalkTimeString();
        _centerCamera();
    }

    public void Confirm()
    {
        if (UIController.MainController.Cur_Path == null)
        {
            throw new System.Exception("Confirm::Confirm: current path not set");
        }

        UIController.Change_State(Main_Controller.App_State.Route);
        UIController.SetWalkTime();
        _centerCamera();
    }

    public void Cancel()
    {
        UIController.MainController.Start_Node = null;
        UIController.MainController.End_Node = null;
        UIController.Change_State(Main_Controller.App_State.Map);
        UIController.Remove_Path();
    }
}
