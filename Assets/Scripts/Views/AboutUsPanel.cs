using com.VisionXR.Controllers;
using com.VisionXR.ModelClasses;
using UnityEngine;

public class AboutUsPanel : MonoBehaviour
{

    [Header(" Game Objects")]
    public AudioDataSO audiodata;
  

    [Header(" Url ")]
    public string discPoolURL;
    public string carromURL;
    public string realityRulerURL;
    public string handPuzzlesURL;
    public string metaGroupURL;



    public void JoinGroupBtnClicked()
    {
        audiodata.PlayAudio(AudioClipType.ButtonClick);
        Application.OpenURL(metaGroupURL);
    }

    public void DiscPoolButtonClicked()
    {
        audiodata.PlayAudio(AudioClipType.ButtonClick);
        OpenOculusStorePDPAndroid(discPoolURL);
    }


    public void CarromButtonClicked()
    {
        audiodata.PlayAudio(AudioClipType.ButtonClick);
        OpenOculusStorePDPAndroid(carromURL);
    }


    public void RealityRulerButtonClicked()
    {
        audiodata.PlayAudio(AudioClipType.ButtonClick);
        OpenOculusStorePDPAndroid(realityRulerURL);
    }

    public void HandPuzzlesButtonClicked()
    {
        audiodata.PlayAudio(AudioClipType.ButtonClick);
        OpenOculusStorePDPAndroid(handPuzzlesURL);
    }

    private static void OpenOculusStorePDPAndroid(string targetAppID)
    {
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject packageManager = currentActivity.Call<AndroidJavaObject>("getPackageManager");
        AndroidJavaObject i = packageManager.Call<AndroidJavaObject>("getLaunchIntentForPackage", "com.oculus.vrshell");
        i.Call<AndroidJavaObject>("setClassName", "com.oculus.vrshell", "com.oculus.vrshell.MainActivity");
        i.Call<AndroidJavaObject>("setAction", "android.intent.action.VIEW");
        i.Call<AndroidJavaObject>("putExtra", "uri", "/item/" + targetAppID);
        i.Call<AndroidJavaObject>("putExtra", "intent_data", "systemux://store");
        currentActivity.Call("startActivity", i);
    }
}
