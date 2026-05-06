using com.VisionXR.ModelClasses;
using UnityEngine;

public class GenericState : StateMachineBehaviour
{
    public UIDataSO uiData;
    public int nextPanelId = 1;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
     
        if (uiData.uiManager != null)
        {

            uiData.uiManager.mainCanvasView.ShowMainPanel(nextPanelId);
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
        if (uiData.uiManager != null)
        {

            uiData.uiManager.mainCanvasView.HideMainPanel(nextPanelId);
        }
    }

}
