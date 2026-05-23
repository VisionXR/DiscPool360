using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using UnityEngine;

public class TestPanelView : MonoBehaviour
{

    [Header("Scriptable Objects")]
    public AudioDataSO audioData;
    public UIDataSO uiData;
    public AchievementsDataSO achievementsData;
    public AchievementManager achievementManager;
  

    public void BackBtnClicked()
    {
        audioData.PlayAudio(AudioClipType.ButtonClick);
        uiData.uiManager.ChangeState("Test", false);
    }

    public void SPBtnClicked()
    {
        audioData.PlayAudio(AudioClipType.ButtonClick);
        Debug.Log("sp clicked");
        uiData.uiManager.ChangeState("Test", false);
        uiData.uiManager.uiController.Play("SinglePlayer");
    }

    public void MPBtnClicked()
    {
        audioData.PlayAudio(AudioClipType.ButtonClick);
        Debug.Log("mp clicked");
        uiData.uiManager.uiController.Play("ChangeDestinationState");
    }

    public void TotalClicked()
    {
        audioData.PlayAudio(AudioClipType.ButtonClick);
     

    }
}
