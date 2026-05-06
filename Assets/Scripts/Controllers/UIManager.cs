using com.VisionXR.ModelClasses;
using com.VisionXR.Views;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.VisionXR.Controllers
{
    public class UIManager : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public UIDataSO uiData;
        public Animator uiController;

        [Header("All Canvas Views")]
        public MainCanvasView mainCanvasView;       
        public PoolCanvasView poolCanvasView;
        public SnookerCanvasView snookerCanvasView;
        public TurnAndFoulCanvasView turnAndFoulCanvasView;
        public InputCanvasView inputCanvasView;

        [Header("Canvas Objects")]
        public List<GameObject> allCanvases;


        private Coroutine hideRoutine;
       


        private void Start()
        {
            ResetCanvases();
          
            uiData.SetUIMachine(this);
            uiController.enabled = true;

        }

        public void ChangeState(string stateName,bool value)
        {
            uiController.SetBool(stateName,value);
           
        }
        /// <summary>
        /// Loops through all parameters and sets every Boolean to false.
        /// </summary>
        public void ResetAllBools()
        {
            if (uiController == null) return;

            // Iterate through all parameters in the Animator Controller
            foreach (AnimatorControllerParameter parameter in uiController.parameters)
            {
                if (parameter.type == AnimatorControllerParameterType.Bool)
                {
                    uiController.SetBool(parameter.name, false);
                }
            }
        }

        public void ShowCanvas(int id)
        {
            if(hideRoutine != null)
            {
               
                StopCoroutine(hideRoutine);
                hideRoutine = null;
            }

            allCanvases[id].SetActive(true);
        }

        public void HideCanvas(int id)
        {
            if (hideRoutine == null)
            {
               hideRoutine =  StartCoroutine(WaitAndHide(id));
            }
        }

        private IEnumerator WaitAndHide(int id)
        {
            yield return new WaitForSeconds(uiData.disableTime);
            allCanvases[id].SetActive(false);
            hideRoutine = null;
        }


        private void ResetCanvases()
        {     
            foreach(GameObject canvas in allCanvases)
            {
                canvas.SetActive(false);
            }
        }

    }
}
