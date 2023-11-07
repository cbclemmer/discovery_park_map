using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Confirm_Controller : MonoBehaviour
{
    public UI_Controller UIController;
    public TMP_Text Start_Text;
    public TMP_Text End_Text;
    public TMP_Text Walk_Text;

    public void Set_Confirm_State(Node_controller endNode) {
        if (UIController.MainController.State != Main_Controller.App_State.Search) return;
        if (UIController.MainController.Start_Node == null) return;
        UIController.MainController.End_Node = endNode;
        UIController.Change_State(Main_Controller.App_State.Confirm);
        Start_Text.text = UIController.MainController.Start_Node.Name;
        End_Text.text = UIController.MainController.End_Node.Name;
        UIController.Create_Path();
        Walk_Text.text = UIController.GetWalkTimeString();
    }

    public void Cancel()
    {
        if (UIController.MainController.State != Main_Controller.App_State.Confirm) return;
        UIController.MainController.Start_Node = null;
        UIController.MainController.End_Node = null;
        UIController.Change_State(Main_Controller.App_State.Map);
        UIController.Remove_Path();
    }
}
