using com.VisionXR.GameElements;
using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace com.VisionXR.Views
{
    public class PoolCanvasView : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public UserDataSO userData;
        public UIDataSO UiData;
        public AudioDataSO audioData;
        public PlayerDataSO playerData;

        [Header("Pool Score Panel View")]
        public PoolScorePanelView FivepoolScorePanelView;
        public PoolScorePanelView EightpoolScorePanelView;

        public GameObject leftfivePoolImages;
        public GameObject rightfivePoolImages;
        public GameObject lefteightPoolImages;
        public GameObject righteightPoolImages;

        [Header("Speaker And MicroPhone  Panels")]
       
        public Image speakerImage;
        public Image microphoneImage;
        public Sprite speakerOnImage;
        public Sprite microphoneOnImage;
        public Sprite speakerOffImage;
        public Sprite microphoneOffImage;

        public PanelOnOff SpeakerPanel;
        public PanelOnOff MicrophonePanel;

        [Header("Navigation Panels")]
        public PanelOnOff leftSideNavigation;
        public PanelOnOff rightSideNavigation;

        [Header("Off Panels")]
        public List<PanelOnOff> panelsToOff;


        private void OnEnable()
        {
            Player mp = playerData.GetMainPlayer();
            if(mp != null)
            {
                PlayerVoiceControl voiceControl = mp.GetComponent<PlayerVoiceControl>();
                if (voiceControl != null)
                {
                    if(voiceControl.recorder.TransmitEnabled)
                    {
                        microphoneImage.sprite = microphoneOnImage;
                    }
                    else
                    {
                        microphoneImage.sprite = microphoneOffImage;
                    }
                }
            }

            Player op = playerData.GetOpponentPlayer();
            if (op != null)
            {
                PlayerVoiceControl voiceControl = op.GetComponent<PlayerVoiceControl>();
                if (voiceControl != null)
                {
                    if (voiceControl.speaker.mute)
                    {
                        speakerImage.sprite = speakerOffImage;
                    }
                    else
                    {
                        speakerImage.sprite = speakerOnImage;
                    }
                }
            }
                  
        }

        public void ShowPoolUI()
        {
            Reset();

            if (userData.myCoins == CoinsType.EightPool)
            {
                EightpoolScorePanelView.enabled = true;
                FivepoolScorePanelView.enabled = false;

                lefteightPoolImages.SetActive(true);
                righteightPoolImages.SetActive(true);
            }
            else if (userData.myCoins == CoinsType.FivePool)
            {
                FivepoolScorePanelView.enabled = true;
                EightpoolScorePanelView.enabled = false;

                leftfivePoolImages.SetActive(true);
                rightfivePoolImages.SetActive(true);
            }


            if(UiData.currentGameType == GameType.MultiPlayer)
            {
                SpeakerPanel.TurnOnPanel();
                MicrophonePanel.TurnOnPanel();
            }
            else
            {
                SpeakerPanel.gameObject.SetActive(false);
                MicrophonePanel.gameObject.SetActive(false);
            }
         
        }

        public void TurnOn()
        {
            foreach (var item in panelsToOff)
            {
                item.TurnOnPanel();
            }

            if (userData.myDominantHand == DominantHand.Left)
            {
              
                rightSideNavigation.TurnOnPanel();
               
            }
            else if (userData.myDominantHand == DominantHand.Right)
            {
  
                leftSideNavigation.TurnOnPanel();
  
            }
        }

        public void TurnOff()
        {
            foreach (var item in panelsToOff)
            {
                item.TurnOffPanel();
            }


            if (userData.myDominantHand == DominantHand.Left)
            {

                rightSideNavigation.TurnOffPanel();

            }
            else if (userData.myDominantHand == DominantHand.Right)
            {

                leftSideNavigation.TurnOffPanel();

            }
        }



        private void Reset()
        {
            leftfivePoolImages.SetActive(false);
            rightfivePoolImages.SetActive(false);
            lefteightPoolImages.SetActive(false);
            righteightPoolImages.SetActive(false);
            FivepoolScorePanelView.enabled = false;
            EightpoolScorePanelView.enabled = false;

        }


        public void SpeakerBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);

            if(speakerImage.sprite == speakerOnImage)
            {
                speakerImage.sprite = speakerOffImage;
                UiData.TurnOffSpeakerEvent?.Invoke();
            }
            else
            {
                speakerImage.sprite = speakerOnImage;
                UiData.TurnOnSpeakerEvent?.Invoke();
            }
           
        }

        public void MicBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
           
            if (microphoneImage.sprite == microphoneOnImage)
            {
                microphoneImage.sprite = microphoneOffImage;
                UiData.TurnOffMicEvent?.Invoke();
            }
            else
            {
                microphoneImage.sprite = microphoneOnImage;
                UiData.TurnOnMicEvent?.Invoke();
            }
        }
    }
}
