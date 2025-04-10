using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Vuforia;
using DataRepositories;
using Models;

namespace Controllers
{
    public sealed class AppController : MonoBehaviour
    {
        private static AppController _instance;
        public string ActiveMenuPanel;
        public bool ShowInstructive = true;

        //public List<Machine> Machines;
        public Machine ActualMachine;
        public string ManualSelected;


        private AppController() //
        {

        }

        public static AppController Instance
        {
            get { return _instance; }
        }

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(this);

                InitConfig();

            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        private void InitConfig()
        {
            //ActiveMenuPanel = "LoginPanel";

            LoadMachines();
        }
        private void Start()
        {

        }

        private void LoadMachines()
        {
            IMachineReposiory ds = new MachineRepository();
            List<Machine> MachineList = ds.GetAll();

            if (MachineList != null)
                ActualMachine = MachineList[0];
        }

        public bool CheckLogin(string name, string pass)
        {
            IUsersRepository ds = new UsersRepositories();
            // name = "Admin";
            // pass = "4321";
            var Usuarios = ds.GetByPass(name, pass);

            if (Usuarios != null)
            {
                return name == Usuarios.Name && pass == Usuarios.Pass;
            }

            return false;
        }

        public void LoadingScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
            //Debug.Log("Loading scene: " + nameScene);
        }

        // public void LoadPanel(GameObject prevPanel, GameObject nextPanel)
        // {
        // ActiveMenuPanel = nextPanel.name;
        // Vector3 position = prevPanel.transform.localPosition;
        // prevPanel.SetActive(false);
        // nextPanel.transform.localPosition = position;
        // nextPanel.SetActive(true);
        // }


        // private bool isTrackingMarker(string imageTargetName)
        // {
        // 
        // var trackable = imageTarget.GetComponent<TrackableBehaviour>();
        // var status = trackable.CurrentStatus;
        // return status == TrackableBehaviour.Status.TRACKED;
        // }
    }

}