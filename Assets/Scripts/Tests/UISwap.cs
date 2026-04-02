using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class UISwap : MonoBehaviour
{
    public Canvas mainCanvas;
    public Canvas tutorialCanvas;
    public InputSystemUIInputModule standaloneInputModule;
    

    public GameObject localAvatar;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       // if(Application.isEditor)
        //{
        //    mainCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        //    tutorialCanvas.renderMode = RenderMode.ScreenSpaceOverlay;

        //    standaloneInputModule.enabled = true;
        //    canvasModule.enabled = false;
        //    mainCanvas.GetComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        //    mainCanvas.GetComponent<CanvasScaler>().matchWidthOrHeight = 1;
        //    mainCanvas.GetComponent<CanvasScaler>().referenceResolution = new Vector2(1024, 688);

        //    tutorialCanvas.GetComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        //    tutorialCanvas.GetComponent<CanvasScaler>().matchWidthOrHeight = 1;
        //    tutorialCanvas.GetComponent<CanvasScaler>().referenceResolution = new Vector2(1024, 688);

        //    localAvatar.SetActive(false);
       // }
    }


}
