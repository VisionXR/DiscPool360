using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.VisionXR.Views
{
    public class AchievementsPanelView : MonoBehaviour
    {

        [Header("Scriptable Objects")]
        public AchievementsDataSO achievementData;
        public AudioDataSO audioData;


        [Header("UI Objects")]
        public GameObject achievementObjectPrefab;
        public Transform generalContentTransform;
        public Transform singlePlayerContentTransform;
        public Transform multiPlayerContentTransform;
        public GameObject generalScrollView;
        public GameObject singlePlayerScrollView;
        public GameObject multiPlayerScrollView;

        [Header("Selection Images")]
        public GameObject singlePlayerSelectionImage;
        public GameObject multiPlayerSelectionImage;
        public GameObject generalSelectionImage;

        [Header("This Objects")]
        public HomePanelView homePanelView;
        public List<PanelOnOff> panelsToOff;
        void OnEnable()
        {
            Initialise();

            achievementData.GotAllAchievementsEvent += Initialise;
        }

        private void OnDisable()
        {
            achievementData.GotAllAchievementsEvent -= Initialise;
        }

        public void Initialise()
        {

            Clear();
            int generalCount = 0;
            int singlePlayerCount = 0;
            int multiPlayerCount = 0;
            var list = achievementData.AllAchievementInfo;
            for (int i = 0; i < list.Count; i++)
            {

                var info = list[i];
                GameObject go;
                if (info.achievementSection == AchievementSection.General)
                {
                    go = Instantiate(achievementObjectPrefab, generalContentTransform);
                    AchievementObject ao = go.GetComponent<AchievementObject>();
                    info.id = (generalCount + 1);
                    ao.SetAchivementInfo(info);
                    generalCount++;
                }
                else if (info.achievementSection == AchievementSection.SinglePlayer)
                {
                    go = Instantiate(achievementObjectPrefab, singlePlayerContentTransform);
                    AchievementObject ao = go.GetComponent<AchievementObject>();
                    info.id = (singlePlayerCount + 1);
                    ao.SetAchivementInfo(info);
                    singlePlayerCount++;
                }
                else
                {
                    go = Instantiate(achievementObjectPrefab, multiPlayerContentTransform);
                    AchievementObject ao = go.GetComponent<AchievementObject>();
                    info.id = (multiPlayerCount + 1);
                    ao.SetAchivementInfo(info);
                    multiPlayerCount++;
                }

            }
        }

        private void Clear()
        {
            // Clear existing entries
            for (int i = generalContentTransform.childCount - 1; i >= 0; i--)
            {
                var child = generalContentTransform.GetChild(i).gameObject;
                if (child != null)
                {
                    Destroy(child);
                }
            }

            for (int i = singlePlayerContentTransform.childCount - 1; i >= 0; i--)
            {
                var child = singlePlayerContentTransform.GetChild(i).gameObject;
                if (child != null)
                {
                    Destroy(child);
                }
            }

            for (int i = multiPlayerContentTransform.childCount - 1; i >= 0; i--)
            {
                var child = multiPlayerContentTransform.GetChild(i).gameObject;
                if (child != null)
                {
                    Destroy(child);
                }
            }
        }

        public void RefreshBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            achievementData.GetAllAchievemnets();
        }


        public void SinglePlayerBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            DisableContent();
            ResetImages();
            singlePlayerSelectionImage.SetActive(true);
            singlePlayerScrollView.SetActive(true);
        }

        public void MultiPlayerBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            DisableContent();
            ResetImages();
            multiPlayerSelectionImage.SetActive(true);
            multiPlayerScrollView.SetActive(true);
        }

        public void GeneralBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
            DisableContent();
            ResetImages();
            generalSelectionImage.SetActive(true);
            generalScrollView.SetActive(true);
        }

        public void BackBtnClicked()
        {
            audioData.PlayAudio(AudioClipType.ButtonClick);
          
        }

        public void TurnOff()
        {
           
        }

        private IEnumerator WaitAndTurnOff()
        {
            yield return new WaitForSeconds(0.5f);
            gameObject.SetActive(false);
        }

        public void TurnOn()
        {
           
        }


        private void DisableContent()
        {
            generalScrollView.SetActive(false);
            multiPlayerScrollView.SetActive(false);
            singlePlayerScrollView.SetActive(false);
        }

        private void ResetImages()
        {
            singlePlayerSelectionImage.SetActive(false);
            multiPlayerSelectionImage.SetActive(false);
            generalSelectionImage.SetActive(false);
        }
    }
}


