using com.VisionXR.Controllers;
using com.VisionXR.ModelClasses;
using UnityEngine;

public class HomeState : StateMachineBehaviour
{
   // state
    public UIDataSO uiData;
    public int homePanelId = 0;
    // local
    private bool isControllerFound;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Entered Home State");

        if(uiData.uiManager != null)
        {
            uiData.uiManager.ShowCanvas(0);
            uiData.uiManager.mainCanvasView.ShowMainPanel(homePanelId);
        }
       
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Exit Home State");
        if (uiData.uiManager != null)
        {
            
            uiData.uiManager.mainCanvasView.HideMainPanel(homePanelId);
        }
    }

   

}
