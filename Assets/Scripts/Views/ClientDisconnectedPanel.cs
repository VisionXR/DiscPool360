using com.VisionXR.Controllers;
using com.VisionXR.ModelClasses;
using UnityEngine;

public class ClientDisconnectedPanel : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public AudioDataSO audioData;
    public UIDataSO uiData;
    public GameDataSO gameData;

    public void HomeBtnClicked()
    {
        audioData.PlayAudio(AudioClipType.ButtonClick);

        gameData.ExitGame();
        gameObject.SetActive(false);
    }
}
