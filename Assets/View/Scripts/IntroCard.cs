using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

namespace Assets.UI
{
    public class IntroCard : MonoBehaviour
    {
        public Image image;
        public Text title;
        public TextMeshProUGUI description;

        public void Init(IntroCardData cardData)
        {
            image.sprite = cardData.cardImage;
            title.text = cardData.cardTitle;
            description.text = cardData.cardDescription;
        }
    }
}