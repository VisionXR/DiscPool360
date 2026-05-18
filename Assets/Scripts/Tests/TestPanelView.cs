using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using UnityEngine;

public class TestPanelView : MonoBehaviour
{

    [Header("Scriptable Objects")]
    public AudioDataSO audioData;
    public UIDataSO uiData;
    public LeaderBoardSO leaderBoardData;
  

    public void BackBtnClicked()
    {
        audioData.PlayAudio(AudioClipType.ButtonClick);
        uiData.uiManager.ChangeState("Test", false);
    }

    public void SPBtnClicked()
    {
        audioData.PlayAudio(AudioClipType.ButtonClick);
        leaderBoardData.SetMyPoints(leaderBoardData.GetApiNameById(0),1);
    }

    public void MPBtnClicked()
    {
        audioData.PlayAudio(AudioClipType.ButtonClick);
        leaderBoardData.SetMyPoints(leaderBoardData.GetApiNameById(1), 1);
    }

    public void TotalClicked()
    {
        audioData.PlayAudio(AudioClipType.ButtonClick);
        leaderBoardData.SetMyPoints(leaderBoardData.GetApiNameById(2), 1);
    }
}
