using com.VisionXR.ModelClasses;
using Fusion;
using UnityEngine;

public class SinglePlayerState : StateMachineBehaviour
{
   
    public UIDataSO uiData;
    public int SinglePlayerPanelId = 1;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Entered SinglePlayer State");
        if (uiData.uiManager != null)
        {
          
            uiData.uiManager.ShowMainPanel(SinglePlayerPanelId);
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
               
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Exited SinglePlayer State");
        if (uiData.uiManager != null)
        {

            uiData.uiManager.HideMainPanel(SinglePlayerPanelId);
        }
    }


}
