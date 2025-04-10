using UnityEngine;


namespace Assets.UI
{
    [CreateAssetMenu(fileName = "IntroCardData", menuName = "New Intro Card Data", order = 0)]
    public class IntroCardData : ScriptableObject
    {
        public Sprite cardImage;
        public string cardTitle;

        [TextArea(3, 5)] // 
        public string cardDescription;
        public int cardIndex;
    }

}