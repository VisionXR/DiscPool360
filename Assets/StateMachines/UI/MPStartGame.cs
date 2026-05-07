using com.VisionXR.ModelClasses;
using UnityEngine;

public class MPStartGame : StateMachineBehaviour
{
   
    public UIDataSO uiData;
    public int mainCanvasId = 0;
    public int poolCanvasId = 1;
    public int SnookerCanvasId = 2;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Entered Start Game State");
        if (uiData.uiManager != null)
        {
            uiData.uiManager.HideCanvas(0);

            if(uiData.currentGameMode == com.VisionXR.HelperClasses.GameMode.Pool)
            {
                uiData.uiManager.ShowCanvas(poolCanvasId);
                uiData.uiManager.poolCanvasView.ShowPoolUI();
                uiData.uiManager.poolCanvasView.TurnOn();
            }
            else if(uiData.currentGameMode == com.VisionXR.HelperClasses.GameMode.Snooker)
            {
                uiData.uiManager.ShowCanvas(SnookerCanvasId);
                uiData.uiManager.snookerCanvasView.TurnOn();
            }
        
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
               
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Exited StartGame State");
        if (uiData.uiManager != null)
        {
            if (uiData.currentGameMode == com.VisionXR.HelperClasses.GameMode.Pool)
            {
                uiData.uiManager.HideCanvas(poolCanvasId);
                uiData.uiManager.poolCanvasView.TurnOff();
            }
            else if (uiData.currentGameMode == com.VisionXR.HelperClasses.GameMode.Snooker)
            {
                uiData.uiManager.HideCanvas(SnookerCanvasId);
                uiData.uiManager.snookerCanvasView.TurnOff();
            }


        }
    }


}
