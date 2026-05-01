using com.VisionXR.ModelClasses;
using Fusion;
using UnityEngine;

public class MultiPlayerState : StateMachineBehaviour
{
   
    public UIDataSO uiData;
    public int multiplayerPanelId = 1;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Entered MultiPlayer State");
        if (uiData.uiManager != null)
        {

            uiData.uiManager.ShowMainPanel(multiplayerPanelId);
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
              
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Exited MultiPlayer State");
        if (uiData.uiManager != null)
        {

            uiData.uiManager.HideMainPanel(multiplayerPanelId);
        }
    }


}
