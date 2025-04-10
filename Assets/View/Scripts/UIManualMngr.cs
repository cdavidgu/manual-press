using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Controllers;
using UnityEngine.UI;
using Models;
using System;
using System.Linq;

namespace View.UI
{
    public class UIManualMngr
    {
        private GameObject Model3D;
        private GameObject IDTagModel;
        private GameObject Infography;
        private GameObject FooterManualController;
        private GameObject PanelDescription;
        private Animator Animation;
        private int manualStepCount = 0;
        private int manualTotalSteps;
        private Dictionary<string, string> manual;
        private UIButton PrevStepButton;
        private UIButton NextStepButton;
        private Text DescriptionText;

        public UIManualMngr(GameObject _Model, GameObject _Infography, GameObject _FooterManualController, GameObject _descPanel)
        {
            SetModel(_Model);
            IDTagModel = Model3D.transform.Find("IDTag").gameObject;
            Infography = _Infography;
            Infography.SetActive(false);
            PanelDescription = _descPanel;
            DescriptionText = PanelDescription.transform.Find("Text").GetComponent<Text>();
            FooterManualController = _FooterManualController;
            FooterManualController.SetActive(false);
            NextStepButton = FooterManualController.transform.GetChild(0).GetComponent<UIButton>();
            PrevStepButton = FooterManualController.transform.GetChild(1).GetComponent<UIButton>();
            NextStepButton.Init(" ", () => NextStep());
            PrevStepButton.Init(" ", () => PrevStep());
            // Animation = Model3D.GetComponent<Animator>();
            // Animation.keepAnimatorControllerStateOnDisable = true;
            // Animation.StopPlayback();
        }
        public void SetModel(GameObject _Model)

        {
            Model3D = _Model;
            IDTagModel = Model3D.transform.Find("IDTag").gameObject;
            Animation = Model3D.GetComponent<Animator>();
            Animation.keepAnimatorStateOnDisable = true;
            Animation.StopPlayback();
        }
        public void ShowManual(Machine ActualMachine, string manualName)
        {
            string animatorControllerFileName = null;
            manualStepCount = 0;
            switch (manualName)
            {
                case "BluePrintButton":
                    manual = ActualMachine.BluePrints;
                    break;
                case "DataSheetButton":
                    manual = ActualMachine.DataSheet;
                    break;
                case "Assemble":
                    manual = ActualMachine.AssembleManual;
                    animatorControllerFileName = "Assembling_Animation_Controller";
                    break;
                case "MaintenanceP1":
                    manual = new Dictionary<string, string>();
                    manual.Add(ActualMachine.MReplacementManual.ElementAt(0).Key, ActualMachine.MReplacementManual.ElementAt(0).Value);
                    manual.Add(ActualMachine.MReplacementManual.ElementAt(1).Key, ActualMachine.MReplacementManual.ElementAt(1).Value);
                    manual.Add(ActualMachine.MReplacementManual.ElementAt(2).Key, ActualMachine.MReplacementManual.ElementAt(2).Value);
                    animatorControllerFileName = "Maintenance_Manual_Animation_Controller";
                    break;
                case "MaintenanceP2":
                    manual = new Dictionary<string, string>();
                    manual.Add(ActualMachine.MReplacementManual.ElementAt(0).Key, ActualMachine.MReplacementManual.ElementAt(0).Value);
                    manual.Add(ActualMachine.MReplacementManual.ElementAt(1).Key, ActualMachine.MReplacementManual.ElementAt(1).Value);
                    manual.Add(ActualMachine.MReplacementManual.ElementAt(3).Key, ActualMachine.MReplacementManual.ElementAt(3).Value);
                    manual.Add(ActualMachine.MReplacementManual.ElementAt(4).Key, ActualMachine.MReplacementManual.ElementAt(4).Value);
                    animatorControllerFileName = "Maintenance_Manual_Animation_Controller";
                    break;
                case "MaintenanceP3":
                    manual = new Dictionary<string, string>();
                    manual.Add(ActualMachine.MReplacementManual.ElementAt(0).Key, ActualMachine.MReplacementManual.ElementAt(0).Value);
                    manual.Add(ActualMachine.MReplacementManual.ElementAt(1).Key, ActualMachine.MReplacementManual.ElementAt(1).Value);
                    manual.Add(ActualMachine.MReplacementManual.ElementAt(3).Key, ActualMachine.MReplacementManual.ElementAt(3).Value);
                    manual.Add(ActualMachine.MReplacementManual.ElementAt(5).Key, ActualMachine.MReplacementManual.ElementAt(5).Value);
                    manual.Add(ActualMachine.MReplacementManual.ElementAt(6).Key, ActualMachine.MReplacementManual.ElementAt(6).Value);
                    animatorControllerFileName = "Maintenance_Manual_Animation_Controller";
                    break;
                case "MaintenanceP4":
                    manual = new Dictionary<string, string>();
                    manual.Add(ActualMachine.MReplacementManual.ElementAt(0).Key, ActualMachine.MReplacementManual.ElementAt(0).Value);
                    manual.Add(ActualMachine.MReplacementManual.ElementAt(1).Key, ActualMachine.MReplacementManual.ElementAt(1).Value);
                    manual.Add(ActualMachine.MReplacementManual.ElementAt(3).Key, ActualMachine.MReplacementManual.ElementAt(3).Value);
                    manual.Add(ActualMachine.MReplacementManual.ElementAt(5).Key, ActualMachine.MReplacementManual.ElementAt(5).Value);
                    manual.Add(ActualMachine.MReplacementManual.ElementAt(7).Key, ActualMachine.MReplacementManual.ElementAt(7).Value);
                    manual.Add(ActualMachine.MReplacementManual.ElementAt(8).Key, ActualMachine.MReplacementManual.ElementAt(8).Value);
                    animatorControllerFileName = "Maintenance_Manual_Animation_Controller";
                    break;
                case "MaintenanceP5":
                    manual = new Dictionary<string, string>();
                    manual.Add(ActualMachine.MReplacementManual.ElementAt(0).Key, ActualMachine.MReplacementManual.ElementAt(0).Value);
                    manual.Add(ActualMachine.MReplacementManual.ElementAt(1).Key, ActualMachine.MReplacementManual.ElementAt(1).Value);
                    manual.Add(ActualMachine.MReplacementManual.ElementAt(3).Key, ActualMachine.MReplacementManual.ElementAt(3).Value);
                    manual.Add(ActualMachine.MReplacementManual.ElementAt(5).Key, ActualMachine.MReplacementManual.ElementAt(5).Value);
                    manual.Add(ActualMachine.MReplacementManual.ElementAt(7).Key, ActualMachine.MReplacementManual.ElementAt(7).Value);
                    manual.Add(ActualMachine.MReplacementManual.ElementAt(9).Key, ActualMachine.MReplacementManual.ElementAt(9).Value);
                    manual.Add(ActualMachine.MReplacementManual.ElementAt(10).Key, ActualMachine.MReplacementManual.ElementAt(10).Value);
                    animatorControllerFileName = "Maintenance_Manual_Animation_Controller";
                    break;
                case "UserManual":
                    manual = ActualMachine.UserManual;
                    animatorControllerFileName = "UserManual_Animation_Controller";
                    break;
                default: break;

            }
            manualTotalSteps = manual.Count;
            Animation.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(ActualMachine.Url + "Animaciones/" + animatorControllerFileName);
            FooterManualController.SetActive(true);
            PrevStepButton.gameObject.SetActive(false);
            IDTagModel.SetActive(false);

            if (manualTotalSteps > 1) NextStepButton.gameObject.SetActive(true);
            else NextStepButton.gameObject.SetActive(false);
            SetStep();
        }


        private void SetStep()
        {
            string actualStep = manual.ElementAt(manualStepCount).Key;
            var resourceName = actualStep.Split('.');
            Debug.Log(actualStep);
            if (actualStep.Contains(".png") || actualStep.Contains(".jpg"))
            {
                PanelDescription.SetActive(false);
                //Debug.Log(resourceName[0]);
                Infography.GetComponent<Image>().sprite = Resources.Load<Sprite>(resourceName[0]);
                Infography.gameObject.SetActive(true);
                Model3D.SetActive(false);
            }
            else
            {
                //Debug.Log(actualStep);
                DescriptionText.text = manual.ElementAt(manualStepCount).Value;
                PanelDescription.SetActive(true);
                Infography.gameObject.SetActive(false);
                Model3D.SetActive(true);
                Animation.Play(actualStep);
            }
        }

        public void NextStep()
        {
            PrevStepButton.gameObject.SetActive(true);
            if (manualStepCount < manualTotalSteps - 1)
            {
                NextStepButton.gameObject.SetActive(true);
                manualStepCount++;
                SetStep();
                Debug.Log("step: " + manualStepCount);
            }
            if (manualStepCount == manualTotalSteps - 1)
                NextStepButton.gameObject.SetActive(false);

        }

        public void PrevStep()
        {

            NextStepButton.gameObject.SetActive(true);
            if (manualStepCount > 0)
            {
                PrevStepButton.gameObject.SetActive(true);
                manualStepCount--;
                SetStep();
                Debug.Log("step: " + manualStepCount);
            }
            if (manualStepCount == 0)
                PrevStepButton.gameObject.SetActive(false);

        }

        public void SetStanByModel()
        {
            Animation.Play("BaseLayer.anim_prensa_completa_entrada");
            IDTagModel.SetActive(true);
        }

        ///  
        /// 

    }
}
