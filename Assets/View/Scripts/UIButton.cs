using System.Collections;
using System.Collections.Generic;
//using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


namespace View.UI
{
    public class UIButton : MonoBehaviour
    {
        public Button button;
        public Image imageButton;
        public Text buttontxt;
        //public Image selectedimage;

        public void Init(Sprite spriteButton, UnityAction callback)
        {
            imageButton.sprite = spriteButton;
            button.onClick.AddListener(callback);
        }
        public void Init(string textButton, UnityAction callback)
        {
            buttontxt.text = textButton;
            button.onClick.AddListener(callback);
        }
        public void Init(string textButton, Sprite spriteButton, UnityAction callback)
        {
            imageButton.sprite = spriteButton;
            button.onClick.AddListener(callback);
        }
        public void Init(BttnInfoContainer buttonInfo)
        {
            this.name = button.name;
            imageButton.sprite = Resources.Load<Sprite>(buttonInfo.ImageName);
            buttontxt.text = buttonInfo.Title;
            button.onClick.AddListener(buttonInfo.Callback);
        }
    }

}