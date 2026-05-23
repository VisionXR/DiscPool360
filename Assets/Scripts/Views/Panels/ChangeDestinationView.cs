using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using UnityEngine;


namespace com.VisionXR.Views
{
    public class ChangeDestinationView : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public AudioDataSO audioData;
        public GameDataSO gameData;
        public UIDataSO uiData;

        [Header(" Local ")]
        public Destination newDestination;
        public DestinationPanelView destinationPanelView;


        public void SetDestination(Destination destination)
        {
            newDestination = destination;
        }

        public void JoinBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
          
            uiData.uiManager.ChangeState("SinglePlayer", false);
            uiData.uiManager.ChangeState("MultiPlayer", false);
            uiData.uiManager.ChangeState("JoinedLobby", false);
            uiData.uiManager.ResetAllBools();
            gameData.ExitGame();

            destinationPanelView.SetDestination(newDestination);
            uiData.uiManager.GoToState(StateName.MPDestinationState);

        }

        public void ResumeBtnClicked()
        {
           
            audioData.PlayAudio(AudioClipType.ButtonClick);
            uiData.uiManager.GoToState(uiData.uiManager.previousStateName);
        }

    }
}
