using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Vuforia;
using Controllers;
using System;
using View.UI;
using Models;
using System.Linq;
using Assets.UI;

//TODO: #urgent Check DB for animations -> replacemente pieces
//TODO: Set Standby state of 3D model when come back to AR menu (when close animations/procidures)
namespace Assets.Controllers
{
    public class ARGUIController : MonoBehaviour
    {
        private Machine ActualMachine;

        [SerializeField]
        UIButton ItemListButtonPrefab;
        [SerializeField]
        GameObject PrensaARPrefab;
        GameObject PopUpMenu;
        GameObject UIFooter;
        GameObject ARInfoPanel;
        GameObject ARMenuPanel, IntroPanel;
        GameObject SelectionModePanel;
        GameObject TargetInstructivePanel;
        GameObject StartButton;
        GameObject WelcomePanel;
        GameObject ImageTarget, SelectedTarget, ModelTarget;
        GameObject MachineModel;
        GameObject MachineModelExplosive;
        GameObject UIInfographyBoundPanel;
        GameObject UIInfography;
        GameObject IDTag;
        UnityAction backAREvent;
        UIManualMngr UIManualMngr;
        GameObject ButtonMenu;
        GameObject DevIDButton;
        GameObject DevDocsButton;
        GameObject DevMaintenanceButton;
        GameObject ARCamera;
        ModelTargetBehaviour modelTargetBehaviour;
        ImageTargetBehaviour imageTargetBehaviour;
        bool imageTargetSelection = true;
        void Awake()
        {
            VuforiaConfiguration.Instance.DeviceTracker.AutoInitAndStartTracker = false;
        }
        // Start is called before the first frame update
        void Start()
        {
            InitScene();
            InitIntroPanel();
            InitSelectionModePanel();
            Init3DModel();
            InitFooter();
            InitManualMngr();
            InitARMenuPanel();

        }

        void InitScene()
        {

            ActualMachine = AppController.Instance.ActualMachine;
            ModelTarget = GameObject.Find("ModelTarget");
            ModelTarget.SetActive(true);
            ImageTarget = GameObject.Find("ImageTarget");
            ImageTarget.SetActive(true);
            ARMenuPanel = GameObject.Find("ARMenuPanel");
            ARMenuPanel.SetActive(false);
            ARInfoPanel = ARMenuPanel.transform.Find("ARInfoPanel").gameObject;
            SelectionModePanel = GameObject.Find("SelectionModePanel");
            SelectionModePanel.SetActive(false);
            TargetInstructivePanel = GameObject.Find("TargetInstructivePanel").gameObject;
            TargetInstructivePanel.SetActive(false);
            GameObject CloseTargetInstBttn = TargetInstructivePanel.transform.Find("OKButton").gameObject;
            CloseTargetInstBttn.GetComponent<UIButton>().Init("OK", () => TargetInstructivePanel.SetActive(false));
            modelTargetBehaviour = ModelTarget.GetComponent<ModelTargetBehaviour>();
            imageTargetBehaviour = ImageTarget.GetComponent<ImageTargetBehaviour>();
            var trackable = ImageTarget.GetComponent<ObserverBehaviour>();
            trackable.OnTargetStatusChanged += OnTrackableStatusChanged;
            trackable = ModelTarget.GetComponent<ObserverBehaviour>();
            trackable.OnTargetStatusChanged += OnTrackableStatusChanged;
            InitWelcomePanel();
        }

        private void InitIntroPanel()
        {
            IntroPanel = GameObject.Find("IntroPanel");
            IntroPanel.SetActive(false);
            IntroPanel.GetComponent<IntroManager>().EndCarrouselAction(() =>
            {
                IntroPanel.SetActive(false);
                SelectionModePanel.SetActive(true);
            });
            // GameObject OKButton = IntroPanel.transform.Find("OKButton").gameObject;
            // OKButton.GetComponent<UIButton>().Init("OK", () =>
            // {
            // SelectionModePanel.SetActive(true);
            // IntroPanel.SetActive(false);
            // });
        }

        void InitSelectionModePanel()
        {
            GameObject ImageTargetBttn = SelectionModePanel.transform.Find("ImageTargetBttn").gameObject;
            GameObject ModelTargetBttn = SelectionModePanel.transform.Find("ModelTargetBttn").gameObject;

            ImageTargetBttn.GetComponent<UIButton>().Init("Image Target", () =>
            {
                imageTargetSelection = true;
                TargetInstructivePanel.SetActive(true);
                InitARMode();
            });
            ModelTargetBttn.GetComponent<UIButton>().Init("Model Target", () =>
            {
                imageTargetSelection = false;
                InitARMode();
            });
        }
        IEnumerator WaitForFrame()
        {
            yield return null;
        }
        void InitARMode()
        {

            // VuforiaBehaviour.Instance.enabled = true;
            VuforiaApplication.Instance.Initialize();
            VuforiaApplication.Instance.OnVuforiaStarted += OnVuforiaInitialized;
            Init3DModel();
            UIManualMngr.SetModel(MachineModel);
            SelectionModePanel.SetActive(false);
        }

        void InitFooter()
        {
            UIFooter = ARMenuPanel.transform.Find("Footer").gameObject;
        }

        void Init3DModel()
        {
            //IDTag = MachineModel.transform.Find("IDTag").gameObject;
            //IDTag.SetActive(false);

            SelectedTarget = imageTargetSelection ? ImageTarget : ModelTarget;
            MachineModel = SelectedTarget.transform.Find("Prensa").gameObject;
            MachineModel.name = ActualMachine.Name;
            MachineModel.SetActive(true);
            MachineModelExplosive = SelectedTarget.transform.Find("PrensaExplosive").gameObject;
            MachineModelExplosive.SetActive(false);
            //MachineModelExplosive.transform.position = MachineModel.transform.position;
        }

        private void InitManualMngr()
        {
            UIInfographyBoundPanel = ARMenuPanel.transform.Find("InfographyBoundPanel").gameObject;
            UIInfography = UIInfographyBoundPanel.transform.Find("Manual_Infography").gameObject;
            UIManualMngr = new UIManualMngr(MachineModel, UIInfography, UIFooter, ARInfoPanel);
        }

        void InitWelcomePanel()
        {
            WelcomePanel = GameObject.Find("WelcomePanel");
            if (AppController.Instance.ShowInstructive) WelcomePanel.SetActive(true);
            else WelcomePanel.SetActive(false);

            StartButton = WelcomePanel.transform.Find("StartButton").gameObject;
            StartButton.GetComponent<UIButton>().Init("Iniciar", () =>
            {
                AppController.Instance.ShowInstructive = false;
                WelcomePanel.SetActive(false);
                IntroPanel.SetActive(true);
            });
        }

        void InitARMenuPanel()
        {

            Sprite spriteCloseBttn = Resources.Load<Sprite>("Sprites/Iconos/Close");
            ButtonMenu = ARMenuPanel.transform.Find("ButtonMenu").gameObject;
            PopUpMenu = ARMenuPanel.transform.Find("PopUpMenu").gameObject;
            PopUpMenu.SetActive(false);
            ARInfoPanel.SetActive(false);
            GameObject CloseButton = ARMenuPanel.transform.Find("CloseButton").gameObject;
            CloseButton.SetActive(false);
            DevIDButton = ButtonMenu.transform.Find("DevIDButton").gameObject;
            DevDocsButton = ButtonMenu.transform.Find("DevDocsButton").gameObject;
            DevMaintenanceButton = ButtonMenu.transform.Find("DevMaintenanceButton").gameObject;
            GameObject RestartButton = ARMenuPanel.transform.Find("RestartButton").gameObject;

            // 

            /* --------------- 
            //TODO: Check how to initialize CloseButton -------------- */

            CloseButton.GetComponent<UIButton>().Init(spriteCloseBttn, () =>
            {
                RestartUIARPanel();
                CloseButton.SetActive(false);
            });
            /* -------------------------------------------------------------------------- */
            RestartButton.GetComponent<UIButton>().Init("", () =>
            {
                CloseButton.SetActive(false);
                ARMenuPanel.SetActive(false);
                RestartUIARPanel();
                RestartToSelection();
            });
            DevIDButton.GetComponent<UIButton>().Init("ID", () =>
            {
                CloseButton.SetActive(true);
                DevDocsButton.SetActive(false);
                DevMaintenanceButton.SetActive(false);
                MachineModel.SetActive(false);
                MachineModelExplosive.SetActive(true);
                //IDTag.SetActive(true);
            });
            DevDocsButton.GetComponent<UIButton>().Init("Docs", () =>
            {
                DevIDButton.SetActive(false);
                DevMaintenanceButton.SetActive(false);
                CloseButton.SetActive(true);
                BttnInfoContainer[] DocsMenuButtonData = new BttnInfoContainer[2];

                DocsMenuButtonData[0] = new BttnInfoContainer("BluePrintsButton", "Planos", "", () =>
                {
                    UIManualMngr.ShowManual(ActualMachine, "BluePrintButton");
                    HideARMenu();
                    PopUpMenu.SetActive(false);
                });

                DocsMenuButtonData[1] = new BttnInfoContainer("DataSheetButton", "Hoja Técnia", "", () =>
                {
                    UIManualMngr.ShowManual(ActualMachine, "DataSheetButton");
                    HideARMenu();
                    PopUpMenu.SetActive(false);
                });

                InitPopUpMenu("Documentos", DocsMenuButtonData);
                PopUpMenu.SetActive(true);
            });
            DevMaintenanceButton.GetComponent<UIButton>().Init("Mntc", () =>
            {
                DevIDButton.SetActive(false);
                DevDocsButton.SetActive(false);
                CloseButton.SetActive(true);
                BttnInfoContainer[] MtncMenuButtonData = new BttnInfoContainer[3];
                BttnInfoContainer[] ReplacementMenuData = new BttnInfoContainer[5];

                ReplacementMenuData[0] = new BttnInfoContainer("ReplacementBttn1", "Tapones", "", () =>
                {
                    UIManualMngr.ShowManual(ActualMachine, "MaintenanceP1");
                    HideARMenu();
                    PopUpMenu.SetActive(false);
                });
                ReplacementMenuData[1] = new BttnInfoContainer("ReplacementBttn2", "Manigueta", "", () =>
                {
                    UIManualMngr.ShowManual(ActualMachine, "MaintenanceP2");
                    HideARMenu();
                    PopUpMenu.SetActive(false);
                });
                ReplacementMenuData[2] = new BttnInfoContainer("ReplacementBttn3", "Varillas Guía", "", () =>
                {
                    UIManualMngr.ShowManual(ActualMachine, "MaintenanceP3");
                    HideARMenu();
                    PopUpMenu.SetActive(false);
                });
                ReplacementMenuData[3] = new BttnInfoContainer("ReplacementBttn4", "Tornillo", "", () =>
                {
                    UIManualMngr.ShowManual(ActualMachine, "MaintenanceP4");
                    HideARMenu();
                    PopUpMenu.SetActive(false);
                });
                ReplacementMenuData[4] = new BttnInfoContainer("ReplacementBttn5", "Corredera", "", () =>
                {
                    UIManualMngr.ShowManual(ActualMachine, "MaintenanceP5");
                    HideARMenu();
                    PopUpMenu.SetActive(false);
                });


                MtncMenuButtonData[0] = new BttnInfoContainer("AssembleButton", "Ensamble", "", () =>
                {
                    UIManualMngr.ShowManual(ActualMachine, "Assemble");
                    HideARMenu();
                    PopUpMenu.SetActive(false);
                });
                MtncMenuButtonData[1] = new BttnInfoContainer("UserManualButton", "Uso", "", () =>
                {
                    UIManualMngr.ShowManual(ActualMachine, "UserManual");
                    HideARMenu();
                    PopUpMenu.SetActive(false);
                });
                MtncMenuButtonData[2] = new BttnInfoContainer("MtncProcessButton", "Mantenimiento", "", () => InitPopUpMenu("Mantenimiento", ReplacementMenuData));

                InitPopUpMenu("Manuales", MtncMenuButtonData);
                PopUpMenu.SetActive(true);
            });

        }

        void HideARMenu()
        {
            DevIDButton.SetActive(false);
            DevDocsButton.SetActive(false);
            DevMaintenanceButton.SetActive(false);
        }
        void RestartUIARPanel()
        {

            DevIDButton.SetActive(true);
            DevDocsButton.SetActive(true);
            DevMaintenanceButton.SetActive(true);
            //backAREvent += () => IDTag.SetActive(false);
            PopUpMenu.SetActive(false);
            UIFooter.SetActive(false);
            ARInfoPanel.SetActive(false);
            UIInfography.SetActive(false);
            MachineModel.SetActive(true);
            MachineModelExplosive.SetActive(false);
            UIManualMngr.SetStanByModel();
        }
        void RestartToSelection()
        {
            SelectionModePanel.SetActive(true);
            // VuforiaBehaviour.Instance.enabled = false;
            VuforiaApplication.Instance.Deinit();
            modelTargetBehaviour.enabled = false;
            imageTargetBehaviour.enabled = false;
        }
        void InitPopUpMenu(string title, BttnInfoContainer[] MenuButtonData)
        {
            UIButton Item;
            GameObject HeaderMenu = PopUpMenu.transform.Find("Header").gameObject;
            HeaderMenu.GetComponent<Text>().text = title;
            GameObject ContenItems = PopUpMenu.transform.Find("ScrollView/Viewport/Content").gameObject;
            Transform ItemPosition = ContenItems.transform;

            foreach (Transform child in ItemPosition)
            {
                Destroy(child.gameObject);
            }
            foreach (var itemInfo in MenuButtonData)
            {
                Item = Instantiate(ItemListButtonPrefab, ItemPosition);
                Item.Init(itemInfo);
            }
        }

        public void OnTrackableStatusChanged(ObserverBehaviour behaviour, TargetStatus statusChangeResult)
        {
            if (statusChangeResult.Status == Status.TRACKED)
            {
                //...
                ARMenuPanel.SetActive(true);
                Debug.Log("Satus = " + statusChangeResult.Status);
            }
            else
            {
                ARMenuPanel.SetActive(false);
                // ARMenuPanel.SetActive(true);
                Debug.Log("Status = " + statusChangeResult.Status);
            }
        }
        void OnVuforiaInitialized()
        {
            Debug.Log("Vuforia Engine initialization finished. :P");
            StartCoroutine(WaitForFrame());
            modelTargetBehaviour.enabled = !imageTargetSelection;
            imageTargetBehaviour.enabled = imageTargetSelection;
        }

    }

}

