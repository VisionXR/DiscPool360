using com.VisionXR.Controllers;
using com.VisionXR.ModelClasses;
using UnityEngine;

namespace com.VisionXR.Views
{
    public class IntroPanelView : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public AudioDataSO audioData;
        public TableDataSO tableData;

        [Header("Game Objects")]
        public GameObject nextPanel;
        public void OkButtonPressed()
        {
            Debug.Log("Ok Button Pressed");
            audioData.PlayAudio(AudioClipType.ButtonClick);
            tableData.PlaceTable();
            gameObject.SetActive(false);
        }
    }
}
