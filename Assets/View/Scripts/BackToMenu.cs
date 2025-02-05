using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Controllers;


namespace View.UI
{
    public class BackToMenu : MonoBehaviour
    {
        [SerializeField]
        ButtonInfoContainer backButtonData;
        UIButton backButton;
        private void Start()
        {
            backButton = gameObject.GetComponent<UIButton>();
            // backButton.Init(backButtonData, () =>
            // {
            // AppController.Instance.ActiveMenuPanel = "ARMenuPanel";
            // AppController.Instance.LoadingScene("MainMenu");
            // 
            // });
        }

    }
}