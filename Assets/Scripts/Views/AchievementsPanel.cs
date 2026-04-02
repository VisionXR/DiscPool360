using com.VisionXR.ModelClasses;
using com.VisionXR.Views;
using System.Reflection;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using com.VisionXR.Controllers;

public class AchievementsPanel : MonoBehaviour
{

    [Header("Scriptable Objects")]
    public AchievementsDataSO achievementData;
    public  AudioDataSO audioData;
   

    [Header("UI Objects")]
    public GameObject achievementObjectPrefab;
    public Transform contentTransform;

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
        if (achievementObjectPrefab == null || contentTransform == null || achievementData == null)
        {
            Debug.LogWarning("AchievementsPanel: missing prefab, contentTransform or achievementData");
            return;
        }

        Clear();


        var list = achievementData.AllAchievementInfo;
        for (int i = 0; i < list.Count; i++)
        {
            var info = list[i];
            GameObject go = Instantiate(achievementObjectPrefab, contentTransform);
            AchievementObject ao = go.GetComponent<AchievementObject>();
            ao.id.text = (i + 1).ToString();
            ao.title.text = list[i].achievementName;
            ao.description.text = list[i].description;
            ao.Icon.sprite = list[i].icon;
            if (list[i].isAchieved)
            {
                ao.lockedObject.SetActive(false);
                ao.unLockedObject.SetActive(true);
            }
            else
            {
                ao.lockedObject.SetActive(true);
                ao.unLockedObject.SetActive(false);
            }
        }
    }

    private void Clear()
    {
        // Clear existing entries
        for (int i = contentTransform.childCount - 1; i >= 0; i--)
        {
            var child = contentTransform.GetChild(i).gameObject;
            if(child != null)
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


}

[Serializable]
public class AchievementUI
{
    public TMP_Text NameText;
    public TMP_Text DescriptionText;
    public GameObject lockedObject;
    public GameObject unLockedObject;

}
