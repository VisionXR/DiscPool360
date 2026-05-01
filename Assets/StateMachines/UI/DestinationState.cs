using com.VisionXR.ModelClasses;
using Fusion;
using UnityEngine;

public class DestinationState : StateMachineBehaviour
{
   
    public UIDataSO uiData;
    public int MainCanvasId = 0;
    public int destinationPanelId = 1;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Entered SinglePlayerBoard State");
        if (uiData.uiManager != null)
        {
            uiData.uiManager.ShowCanvas(MainCanvasId);
            uiData.uiManager.mainCanvasView.ShowMainPanel(destinationPanelId);
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
               
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Exited SinglePlayerBoardState");
        if (uiData.uiManager != null)
        {

            uiData.uiManager.mainCanvasView.HideMainPanel(destinationPanelId);
            
        }
    }


}
