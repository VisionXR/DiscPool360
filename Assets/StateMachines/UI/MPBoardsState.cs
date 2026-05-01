using com.VisionXR.ModelClasses;
using Fusion;
using UnityEngine;

public class MPBoardsState : StateMachineBehaviour
{
   
    public UIDataSO uiData;
    public int MainCanvasId = 0;
    public int BoardsPanelId = 1;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Entered SinglePlayerBoard State");
        if (uiData.uiManager != null)
        {
            uiData.uiManager.ShowCanvas(MainCanvasId);
            uiData.uiManager.mainCanvasView.ShowMainPanel(BoardsPanelId);
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

            uiData.uiManager.mainCanvasView.HideMainPanel(BoardsPanelId);
            
        }
    }


}
