using com.VisionXR.ModelClasses;
using Fusion;
using UnityEngine;

public class SPState : StateMachineBehaviour
{
   
    public UIDataSO uiData;
    public int SinglePlayerPanelId = 1;
    public int MainCanvasId = 0;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        if (uiData.uiManager != null)
        {
            uiData.uiManager.ShowCanvas(MainCanvasId);
            uiData.uiManager.mainCanvasView.ShowMainPanel(SinglePlayerPanelId);
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
               
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
        if (uiData.uiManager != null)
        {
            uiData.uiManager.mainCanvasView.HideMainPanel(SinglePlayerPanelId);
        }
    }


}
