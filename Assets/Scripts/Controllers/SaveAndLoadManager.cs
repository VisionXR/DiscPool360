using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System;
using System.Collections;
using System.IO;
using UnityEngine;

namespace com.VisionXR.Controllers
{
    public class SaveAndLoadManager : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public UserDataSO userData;
        public SaveDataSO saveData;

        [Header("Other Objects")]
        public AudioSource BGAudioSource;
        public string settingsKey = "DiscPoolSettings";


        private void OnEnable()
        {
            saveData.SaveSettingsEvent += SaveSettings;
            saveData.LoadSettingsEvent += LoadSettings;
        }

        private void OnDisable()
        {
            saveData.SaveSettingsEvent -= SaveSettings;
            saveData.LoadSettingsEvent -= LoadSettings;
        }

        IEnumerator Start()
        {
            yield return new WaitForSeconds(1);
            LoadSettings();
           
        }

        public void SaveSettings(PlayerSettings playerSettings)
        {
            
            try
            {
                string json = JsonUtility.ToJson(playerSettings);
                string path = Path.Combine(Application.persistentDataPath, settingsKey + ".txt");
                File.WriteAllText(path, json);
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to save user data to file: {e.Message}");
            }
        }

        public void LoadSettings()
        {
            PlayerSettings settings = null;
            try
            {
                
                string path = Path.Combine(Application.persistentDataPath, settingsKey + ".txt");
                if (File.Exists(path))
                {
                    string json = File.ReadAllText(path);
                    settings = JsonUtility.FromJson<PlayerSettings>(json);
                }


            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to load user data from file: {e.Message}");

            }

            if (settings != null)
            {
                BGAudioSource.volume = settings.musicVolume;


            }
        }
    }


}
