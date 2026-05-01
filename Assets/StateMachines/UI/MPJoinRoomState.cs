using com.VisionXR.ModelClasses;
using Fusion;
using UnityEngine;

public class MPJoinRoomState : StateMachineBehaviour
{
   
    public UIDataSO uiData;
    public int joinRoomPanelId = 1;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Entered Join room State");
        if (uiData.uiManager != null)
        {

            uiData.uiManager.mainCanvasView.ShowMainPanel(joinRoomPanelId);
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
              
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Exited Join room State");
        if (uiData.uiManager != null)
        {

            uiData.uiManager.mainCanvasView.HideMainPanel(joinRoomPanelId);
        }
    }


}
