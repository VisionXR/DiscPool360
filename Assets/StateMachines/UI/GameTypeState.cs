using com.VisionXR.ModelClasses;
using Fusion;
using UnityEngine;

public class GameTypeState : StateMachineBehaviour
{
   
    public UIDataSO uiData;
    public int gameTypepanelId = 1;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Entered GameType State");
        if (uiData.uiManager != null)
        {

            uiData.uiManager.mainCanvasView.ShowMainPanel(gameTypepanelId);
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
              
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Exited GameType State");
        if (uiData.uiManager != null)
        {

            uiData.uiManager.mainCanvasView.HideMainPanel(gameTypepanelId);
        }
    }


}
